using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDB : MonoBehaviour
{
    static Dictionary<string, monsterBase> monsters;

    public static void Init()
    {//在所有路径下加载monsterBase存入队列

        monsters = new Dictionary<string, monsterBase>();

        var monsterArray = Resources.LoadAll<monsterBase>("Monsters");
        Debug.Log($"获取队列：{monsterArray.ToString()}");

        foreach (var monster in monsterArray)
        {//将所有monsterBase以名称为索引写入字典
            if (monsters.ContainsKey(monster.name)) 
            { 
                Debug.Log("取消保存重复内容");
                continue;
            }
            monsters[monster.Name] = monster;
            Debug.Log($"已保存名为{monster.name}的模板");
        }
    }
    public static monsterBase GetMonsterByName(string name)
    {
        if (!monsters.ContainsKey(name))
        {
            Debug.LogError($"未发现名称为{name}的monsterbase");
            return null;
        }

        return monsters[name];
    }
}
