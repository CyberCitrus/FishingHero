using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] Flowchart flowchart;
    [SerializeField] battleSystem battleSystem;

    int fishTime;
    bool isFirstBattle = true;

    private void Start()
    {
        fishTime = flowchart.GetIntegerVariable("fishTime");
        isFirstBattle = flowchart.GetBooleanVariable("firstBattle");
        battleSystem.OnBattleOver += AfterBattle;
    }

    void AfterBattle(bool won)
    {
        if (won)
        {
            flowchart.SetIntegerVariable("fishTime", ++fishTime);
            Debug.Log($"现在的钓鱼次数是{fishTime}");
        }
        if(isFirstBattle)
        {
            flowchart.SetBooleanVariable("firstBattle", false);
            flowchart.SendFungusMessage("初次战斗");
        }
    }
}
