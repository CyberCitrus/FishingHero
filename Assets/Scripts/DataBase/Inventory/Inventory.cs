using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour,ISavable
{
    [SerializeField] public List<ItemSlot> slots;

    public event Action onUpdate;

    public List<ItemSlot> Slots => slots;

    public ItemBase UseItem(int itemIndex, Monster seletedMember)
    {
        var item = slots[itemIndex].Item;
        bool itemUsed = item.Use(seletedMember);
        if (itemUsed)
        {
            RemoveItem(item);
            return item;
        }
        return null;
    }

    public void RemoveItem(ItemBase item)
    {
        var itemSlot = slots.First(slots => slots.Item == item);
        itemSlot.Count-=1;
        if(itemSlot.Count == 0)
        {
            slots.Remove(itemSlot);
        }
        onUpdate?.Invoke();
    }

    public void AddItem(ItemBase item, int count=1)
    {
        var currentSlot = Slots;
        var itemSlot = slots.First(slots=> slots.Item == item);
        if(itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlot.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }

        onUpdate?.Invoke();
    }
    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerMovement>().GetComponent<Inventory>();
    }

    public object CaptureState()
    {
        var saveData = new InventorySaveData()
        {//遍历保存每个物品栏的内容
            items = slots.Select(i => i.GetSaveData()).ToList(),
        };

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;
        //遍历重构物品栏内容
        slots = saveData.items.Select(i => new ItemSlot(i)).ToList();

        onUpdate?.Invoke();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemSlot(ItemSaveData saveData=null)
    {
        item = ItemDB.GetItemByName(saveData.name);
        count = saveData.count;
    }

    public ItemBase Item {
        get => item;
        set => item = value;
            }
    public int Count 
    {
        get => count;
        set => count = value;
    }
    public ItemSaveData GetSaveData()
    {
        var saveData = new ItemSaveData()
        {
            name = item.Name,
            count = count
        };
        return saveData;
    }
}

[Serializable]
public class ItemSaveData
{
    public string name;
    public int count;
}
[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items;
}