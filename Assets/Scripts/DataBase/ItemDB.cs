using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    static Dictionary<string, ItemBase> Items;
    static Dictionary<int, ItemBase> ItemsByID;

    public static void Init()
    {//在指定路径下加载ItemBase存入队列

        Items = new Dictionary<string, ItemBase>();
        ItemsByID = new Dictionary<int, ItemBase>();
        int ID = 0;

        var ItemArray = Resources.LoadAll<ItemBase>("");
        Debug.Log($"获取队列：{ItemArray.ToString()}");

        foreach (var Item in ItemArray)
        {//将所有ItemBase以名称为索引写入字典
            if (Items.ContainsKey(Item.name))
            {
                Debug.Log("取消保存重复内容");
                continue;
            }
            Items[Item.Name] = Item;
            Debug.Log($"已保存名为{Item.Name}的模板");
        }
        foreach(var Item in ItemArray)
        {//将所有ItemBase按顺序以ID为索引写入字典
            ItemsByID[ID] = Item;
            ID++;
            Debug.Log($"将{Item.Name}保存为{ID}号物品");
        }
    }
    public static ItemBase GetItemByName(string name)
    {
        if (!Items.ContainsKey(name))
        {
            Debug.LogError($"未发现名称为{name}的Itembase");
            return null;
        }

        return Items[name];
    }
    public static ItemBase GetItemByID(int id)
    {
        if (!ItemsByID.ContainsKey(id))
        {
            Debug.LogError($"未发现{id}号物品");
            return null;
        }
        return ItemsByID[id];
    }
}
