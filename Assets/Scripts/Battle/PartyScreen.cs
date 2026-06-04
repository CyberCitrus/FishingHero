using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    PartyMemberUI[] memberSlots;
    List<Monster> monsters;
    MonsterParty party;
    
    int selection = 0;

    public Monster selectionMember => monsters[selection];

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
        party = MonsterParty.GetPlayerParty();
        monsters = party.Monsters;
        SetPartyData(monsters);
        Debug.Log($"获取成员UI数量：{memberSlots.Length}，获取成员列表数量：{monsters.Count}");
    }

    public void SetPartyData(List<Monster> monsters)
    {
        //this.monsters = party.Monsters;
        for(int i = 0; i < memberSlots.Length; i++)
        {
            if (i < monsters.Count)
            {
                memberSlots[i].Init(monsters[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }
        messageText.text = "选择使用对象";
    }

    public void HandleUpdate(Action onSelected, Action onBack)
    {
        //Debug.Log($"selection={selection}");
        int prevSelection = selection;
        UpdateMemberSelection(prevSelection);
        if (Input.GetButtonDown("Up"))
        {
            --selection;
            selection = Mathf.Clamp(selection, 0, monsters.Count - 1);
        }
        else if (Input.GetButtonDown("Down"))
        {
            ++selection;
            selection = Mathf.Clamp(selection, 0, monsters.Count - 1);
        }

        if(selection != prevSelection)
            UpdateMemberSelection(selection);

        if (Input.GetButtonDown("Confirm"))
        {
            onSelected?.Invoke();
        }
        else if (Input.GetButtonDown("Back"))
        {
            onBack?.Invoke();
        }
    }
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
