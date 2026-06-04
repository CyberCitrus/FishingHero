using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Monster
{
    [SerializeField] monsterBase _base;
    [SerializeField] int _level;

    public event Action OnHPChanged;
    public monsterBase Base { 
        get {
            return _base;
        } 
    }
    public int level {
        get
        {
            return _level;
        }
        }
    public int ExpNow { get; set; }
    public int HP { get; set; }
    public string Name { get; set; }

    public List<Move> moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Monster(MonsterSaveData saveData)
    {
        _base = MonsterDB.GetMonsterByName(saveData.name);
        Name = saveData.name;
        HP = saveData.HP;
        ExpNow = saveData.expNow;
        _level = saveData.level;

        moves = new List<Move>();
        for (int i = 0; i < Base.Move.Count; i++)
        {
            moves.Add(new Move(Base.Move[i]));
        }
        CalculateStats();
    }
    public void Init()
    {
        Name = Base.Name;
        moves = new List<Move>();
            for (int i = 0; i < Base.Move.Count; i++)
        {
            moves.Add(new Move(Base.Move[i]));
        }
        ExpNow = Base.GetExpForLevel(level);
        CalculateStats();
        HP = MaxHp;
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * level / 100f) + 5));
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * level / 100f) + 5));
        MaxHp = Mathf.FloorToInt((Base.MaxHP * level / 100f) + 10);
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];
        return statVal;
    }
    //返回根据等级计算的数值
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }
    public int MaxHp { get; private set; }
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }
    public void DecreaseHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        OnHPChanged?.Invoke();
    }
    public void IncreaseHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHp);
        OnHPChanged?.Invoke();
    }

    public bool CheckForLevelUp()
    {
        if(ExpNow > Base.GetExpForLevel(level + 1))
        {
            ++_level;
            return true;
        }
        return false;
    }
    public DamageDetails TakeDamage(Move move, Monster attacker)
    {//先取0.85-1的随机数，再将等级权重与攻击方的能力、技能威力相乘（速度快的怪物则伤害较低），最终乘以随机数取整作为伤害
        //返回值用于判断受击方是否被击败

        float critical = 1f;
        //如果0-100的随机数小于6.25，造成双倍的暴击伤害
        if(UnityEngine.Random.value * 100f <= 6.25f)
        {
            critical = 2f;
        }
        var damageDetails = new DamageDetails()
        {
            Critical = critical,
            isDefeated = false
        };
        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.level + 10) / 250f;
        float d = a * move.mbase.Power * ((float)attacker.Attack / Speed) + 2 * critical;
        int damage = Mathf.FloorToInt(d * modifiers);
        DecreaseHP(damage);
        if(HP <= 0)
        {//受击方被击败
            HP = 0;
            damageDetails.isDefeated = true;
        }

        return damageDetails;
    }
    public Move GetRandomMove()
    {
        int r = UnityEngine.Random.Range(0, moves.Count);
        return moves[r];
    }

public MonsterSaveData GetSaveDataPlayer()
{
        var saveData = new MonsterSaveData()
        {
            HP = HP,
            level = level,
            name = Name,
            expNow = ExpNow
        };
    return saveData;
}
}
    public class DamageDetails
    {
        public bool isDefeated { get; set; }
        public float Critical { get; set; }

    }


[System.Serializable]
public class MonsterSaveData
{
    public string name;
    public int HP;
    public int level;
    public int expNow;
}