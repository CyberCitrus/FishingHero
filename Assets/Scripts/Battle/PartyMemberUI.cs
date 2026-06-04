using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;

    Monster _monster;

    public void Init(Monster monster)
    {
        _monster = monster;
        UpdateData();

        _monster.OnHPChanged += UpdateData;
    }
    void UpdateData()
    {
        nameText.text = _monster.Base.Name;
        levelText.text = "Lv." + _monster.level;
        hpBar.setHP((float)_monster.HP / _monster.MaxHp);
    }
    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = Color.yellow;
        else
            nameText.color = Color.white;
    }
}
