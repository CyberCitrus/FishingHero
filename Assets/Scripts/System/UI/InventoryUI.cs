using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryUIState { ItemSelection, PartySelection, Busy}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI Info;

    [SerializeField] PartyScreen partyScreen;

    Action onItemUsed;

    List<ItemSlotUI> slotUIList;
    Inventory inventory;

    int selectedItem = 0;
    InventoryUIState state;

    private void Awake()
    {
        inventory =  Inventory.GetInventory();
    }
    private void Start()
    {
         UpdateItemList();
         partyScreen.Init();
         inventory.onUpdate += UpdateItemList;
    }
    public void HandleUpdate(Action onBack, Action onItemUsed = null)
    {
        this.onItemUsed = onItemUsed;
        if (state == InventoryUIState.ItemSelection)
        {
            int preSelection = selectedItem;
            if (Input.GetButtonDown("Down"))
            {
                ++selectedItem;
            }
            else if (Input.GetButtonDown("Up"))
            {
                --selectedItem;
            }

            selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

            if (preSelection != selectedItem)
                UpdateItemSelection();
            if (Input.GetButtonDown("Confirm"))
            {
                OpenPartyScreen();
            }
            if (Input.GetButtonDown("Back"))
            {
                onBack?.Invoke();
            }
        }
        else if (state == InventoryUIState.PartySelection)
        {
            Action onSelected = () =>
            {
                bool used = inventory.UseItem(selectedItem, partyScreen.selectionMember);
                inventory.UseItem(selectedItem, partyScreen.selectionMember);
                if(used)
                {
                    partyScreen.gameObject.SetActive(false);
                    onItemUsed?.Invoke();
                }
            };
            Action onBackPartyScreen = () =>
            {
                ClosePartyScreen();
            };
            partyScreen.HandleUpdate(onSelected, onBackPartyScreen);
        }
    }
    private void UpdateItemList()
    {
        foreach(Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        slotUIList = new List<ItemSlotUI>();

        foreach(var itemSlot in inventory.Slots)
        {
            var slotUIobj = Instantiate(itemSlotUI, itemList.transform);
            slotUIobj.SetData(itemSlot);

            slotUIList.Add(slotUIobj);
        }

        UpdateItemSelection();
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].NameText.color = Color.yellow;
            else
                slotUIList[i].NameText.color = Color.white;
        }

        var item = inventory.Slots[selectedItem].Item;
        itemIcon.sprite = item.Icon;
        Info.text = item.Description;
    }
    void OpenPartyScreen()
    {
        state = InventoryUIState.PartySelection;
        partyScreen.gameObject.SetActive(true);
    }
    void ClosePartyScreen()
    {
        partyScreen.gameObject.SetActive(false);
        state = InventoryUIState.ItemSelection;
    }
}
