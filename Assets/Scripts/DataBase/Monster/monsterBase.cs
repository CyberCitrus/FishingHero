using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster")]
[System.Serializable]
public class monsterBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] Sprite frontSprite;

    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int speed;
    [SerializeField] int level;
    [SerializeField] int exp;
    [SerializeField] List<moveBase> move;
    //通过属性调取数据，使用方法与变量相同
    public string Name { get { return name; } }
    public Sprite Sprite { get { return frontSprite; } }
    public int MaxHP { get { return maxHP; } }
    public int Attack { get { return attack; } }
    public int Speed { get { return speed; } }
    public int Exp { get { return exp; } }
    public int GetExpForLevel(int level)
    {
        this.level = level;
        return level * level * level;
    }
    public List<moveBase> Move { get {  return move; } }

}
public enum Stat
{
    Attack,
    Speed
}