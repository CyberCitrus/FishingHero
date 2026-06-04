using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;


[CommandInfo("MyScripts",
             "Fish",
             "向玩家背包添加一个恢复药")]
public class FungusFish : Command
{
    private ItemBase m_Item;
    public override void OnEnter()
    {
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        Debug.Log($"获取{player.name}");
        int r = Random.Range(0, 2);
        m_Item = ItemDB.GetItemByID(r);
        Debug.Log($"{m_Item.Name}已添加，随机数为{r}");
        player.GetComponent<Inventory>().AddItem(m_Item);
        
        Continue();
    }
}
