using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Create new recovery item")]
public class RecoveryItem : ItemBase
{
    [Header("HP")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHP;

    public override bool Use(Monster monster)
    {
        if ( restoreMaxHP || hpAmount > 0)
        {
            if(monster.HP == monster.MaxHp)
            {
                return false;
            }
            if(restoreMaxHP)
                monster.IncreaseHP(monster.MaxHp);
            else
            monster.IncreaseHP(hpAmount);
        }
        return true;
    }
}
