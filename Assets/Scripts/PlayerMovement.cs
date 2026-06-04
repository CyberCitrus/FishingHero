using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Fungus;

public class PlayerMovement : MonoBehaviour, ISavable
{
    Rigidbody2D rb;
    Collider2D coll;
    Animator anim;
    [SerializeField] Flowchart flowchart;
    //public GameObject myBag;
    //bool isOpen;

    public event Action<GameObject> OnEncounter;
    public float speed;
    Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        Movement();
        SwitchAnim();
        //OpenMyBag();
    }

    void Movement()//移动
    {
        //Debug.Log("输入移动");
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

    }

    void SwitchAnim()//切换动画
    {
        if (movement != Vector2.zero)//保证Horizontal归0时，保留movment的值来切换idle动画的blend tree
        {
            anim.SetFloat("horizontal", movement.x);
            anim.SetFloat("vertical", movement.y);
        }
        anim.SetFloat("speed", movement.magnitude);//magnitude 也可以用 sqrMagnitude 具体可以参考Api 默认返回值永远>=0
    }

    private void CheckForEncouters(GameObject obj)
    {
        if (UnityEngine.Random.Range(1, 101) <= 50)
        {
            OnEncounter(obj);
            Debug.Log("触发战斗");
        }
    }

    public object CaptureState()
    {//返回应当保存的内容
        var saveData = new PlayerSaveData()
        {
            position = new float[] { transform.position.x, transform.position.y },
            playerData = GetComponent<MonsterParty>().Monsters.Select(p => p.GetSaveDataPlayer()).ToList(),
            fishTime = flowchart.GetIntegerVariable("fishTime")
        };
        Debug.Log("获取MonsterParty");
        return saveData;
    }

    public void RestoreState(object state)
    {//恢复保存的内容
        var saveData = (PlayerSaveData)state;
        var pos = saveData.position;
        transform.position = new Vector3(pos[0], pos[1]);
        Debug.Log("恢复位置");
        //恢复玩家状态（HP，等级）
        //传入saveData重构Monster数据
        GetComponent<MonsterParty>().Monsters = saveData.playerData.Select(s => new Monster(s)).ToList();
        Debug.Log("恢复玩家状态");

        //恢复任务对话条件
        flowchart.SetIntegerVariable("fishTime", saveData.fishTime);
        Debug.Log($"钓鱼次数恢复到{flowchart.GetIntegerVariable("fishTime")}");
        flowchart.SetBooleanVariable("firstBattle", saveData.firstBattle);
        Debug.Log($"初次战斗状态恢复到{flowchart.GetBooleanVariable("firstBattle")}");
    }
    //void OpenMyBag()
    //{
    //    if(Input.GetKeyDown(KeyCode.I)) 
    //    {
    //        isOpen = !isOpen;
    //        myBag.SetActive(isOpen);
    //    }
    //}
}

[Serializable]
public class PlayerSaveData
{
    public float[] position;
    public List<MonsterSaveData> playerData;
    public int fishTime;
    public bool firstBattle;
}