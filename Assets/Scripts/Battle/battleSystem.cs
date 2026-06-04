using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { Start, ActionSelection, MoveSelection, Bag, PerformMove, Busy, battleOver}
public class battleSystem : MonoBehaviour
{
    [SerializeField] battleUnit playerUnit;
    [SerializeField] battleHUD playerHUD;
    [SerializeField] battleUnit enemyUnit;
    [SerializeField] battleHUD enemyHUD;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] InventoryUI inventoryUI;
    

    public event Action<bool> OnBattleOver;

    BattleState state;
    //记录玩家选择的行动（战斗/逃跑）
    int currentAction;
    int currentMove;
    int escapeCount;

    MonsterParty playerParty;
    Monster wildMonster;
    public void StartBattle(MonsterParty playerParty, Monster wildMonster)
    {
        this.playerParty = playerParty;
        this.wildMonster = wildMonster;
        //协程运行，牺牲少量性能保证函数被执行
        StartCoroutine(SetupBattle());
    }
    public void HandleUpdate()
    {//随时根据状态切换对话框信息
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = BattleState.ActionSelection;
            };

            Action onItemUsed = () => 
            {//唤起道具使用后事件
                Debug.Log("使用完毕");
                state = BattleState.Busy;
                inventoryUI.gameObject.SetActive(false);
                MoveSelection();
            };
            inventoryUI.HandleUpdate(onBack, onItemUsed);
        }
    }
    void ItemUsed()
    {
        inventoryUI.gameObject.SetActive(false);
        dialogBox.EnableActionSelector(false);
        new WaitForSeconds(1f);
        EnemyMove();
    }
    void HandleActionSelection()
    {//键盘控制选择行动
        if (Input.GetButtonDown("Down"))
        {
            if (currentAction < 2) currentAction++;
        }
        else if (Input.GetButtonDown("Up")) 
        {
            if (currentAction > 0) currentAction--; 
        }
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetButtonDown("Confirm"))
        {
            if(currentAction == 0)
            {//战斗
                MoveSelection();
            }
            else if(currentAction == 1)
            {//逃跑
                StartCoroutine(TryToEscape());
            }
            else if(currentAction == 2)
            {//道具
                OpenBag();
            }
        }
    }
    void HandleMoveSelection()
    {
        if (Input.GetButtonDown("Down"))
        {
            if (currentMove < playerUnit.monster.moves.Count-1 && currentMove >= 0) currentMove++;
        }
        else if (Input.GetButtonDown("Up"))
        {
            if (currentMove > 0 && currentMove < playerUnit.monster.moves.Count) currentMove--;
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.monster.moves[currentMove]);

        if (Input.GetButtonDown("Confirm"))
        {//如果确认使用某一动作则切换UI并执行玩家选择的动作
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableMoveDetails(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
    }

    void OpenBag()
    {
        state = BattleState.Bag;
        inventoryUI.gameObject.SetActive(true);
    }
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {//如果造成特殊效果，显示文字说明
        if(damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("暴击！造成双倍伤害");
        }
    }

    IEnumerator RunMove(battleUnit sourceUnit, battleUnit targetUnit, Move move)
    {//执行动作
        yield return dialogBox.TypeDialog($"{sourceUnit.monster.Name}摆出了架势......");
        yield return dialogBox.TypeDialog($"{sourceUnit.monster.Name}使用了{sourceUnit.monster.moves[currentMove].mbase.Name}！");
        var damageDetails = targetUnit.monster.TakeDamage(move, sourceUnit.monster);
        yield return enemyHUD.UpdateHPAsync();
        yield return playerHUD.UpdateHPAsync();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.isDefeated)
        {
            //yield return dialogBox.TypeDialog($"{targetUnit.monster.Name}被击败了！");

            //yield return new WaitForSeconds(2f);

            //BattleOver(sourceUnit.isPlayer);
            yield return HandleBattleOver(targetUnit);
        }
    }

    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        Debug.Log("执行逃跑");
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(true);
        ++escapeCount;
        int playerSpeed = playerUnit.monster.Speed;
        int enemySpeed  = enemyUnit.monster.Speed;

        if(enemySpeed < playerSpeed)
        {
            yield return dialogBox.TypeDialog($"成功逃脱了！");
            BattleOver(false);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeCount;
            f = f % 256;
            if (UnityEngine.Random.Range(0, 256) < f)
            {
                yield return dialogBox.TypeDialog($"成功逃脱了！");
                BattleOver(false);
            }
            else
            {
                yield return dialogBox.TypeDialog($"对方很警惕，逃跑失败了！");
                StartCoroutine(EnemyMove());
            }
        }
    }

    IEnumerator HandleBattleOver(battleUnit defeatedUnit)
    {
        yield return dialogBox.TypeDialog($"{defeatedUnit.monster.Name}被击败了！");
        yield return new WaitForSeconds(2f);

        if (!defeatedUnit.isPlayer)
        {
            //获得EXP
            int exp = defeatedUnit.monster.Base.Exp;
            int enemylevel = defeatedUnit.monster.level;
            int expGain = Mathf.FloorToInt( (exp * enemylevel) / 7 );
            playerUnit.monster.ExpNow += expGain;
            yield return dialogBox.TypeDialog($"{playerUnit.monster.Name}获得了{expGain}点经验值！");
            yield return playerHUD.SetExpSmooth();

            while (playerUnit.monster.CheckForLevelUp())
            {
                playerHUD.SetLevel();
                yield return dialogBox.TypeDialog($"{playerUnit.monster.Name}的等级提升到了{playerUnit.monster.level}！");
                yield return playerHUD.SetExpSmooth(true);
            }

            yield return new WaitForSeconds(1f);
        }
        BattleOver(!defeatedUnit.isPlayer);
    }
    void BattleOver(bool isPlayer)
    {
        state = BattleState.battleOver;
        playerHUD.ClearData();
        enemyHUD.ClearData();
        OnBattleOver(isPlayer);
    }
    public IEnumerator SetupBattle()
    {//设置界面初始状态和玩家、敌人的数据
        playerUnit.Setup(playerParty.GetUseable());
        playerHUD.setData(playerUnit.monster);
        enemyUnit.Setup(wildMonster);
        enemyHUD.setData(enemyUnit.monster);
        escapeCount = 0;
        currentAction = 0;
        currentMove = 0;

        dialogBox.SetMoveNames(playerUnit.monster.moves);
        //$后的字符串可以添加变量
        //yield内容执行完毕后才会继续执行，保证每次对话框中的文字显示完毕再执行下一命令
        yield return dialogBox.TypeDialog($"{enemyUnit.monster.Base.Name}出现了！");
        //切换状态
        PlayerAction();
    }
    IEnumerator PlayerMove()
    {//执行玩家选择的动作
        //为阻止玩家在执行动作期间改变currentMove，将状态设置为Busy，停止接收玩家的操作
        state = BattleState.PerformMove;
        var move = playerUnit.monster.moves[currentMove];
        yield return RunMove(playerUnit, enemyUnit, move);
        if (state == BattleState.PerformMove)
            StartCoroutine(EnemyMove());
    }

    void PlayerAction()
    {//玩家回合，选择战斗或逃跑
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("选择一个行动"));
        dialogBox.EnableActionSelector(true);
    }
    void MoveSelection()
    {//玩家回合，选择要使用的动作
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
        dialogBox.EnableMoveDetails(true);
    }
    IEnumerator EnemyMove()
    {//敌人随机进行一次行动，然后切换到玩家回合
        state = BattleState.PerformMove;
        var move = enemyUnit.monster.GetRandomMove();
        yield return RunMove(enemyUnit, playerUnit, move);
        if(state == BattleState.PerformMove)
            PlayerAction();
    }

}
