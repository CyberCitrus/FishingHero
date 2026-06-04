using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MonsterParty : MonoBehaviour
{
    [SerializeField] public List<Monster> monsters;

    public event Action OnUpdated;

    public List<Monster> Monsters
    {
        get { return monsters; }
        set { monsters = value; }
    }
    private void Start()
    {
        foreach (var monster in monsters)
        {
            monster.Init();
        }
    }
    public Monster GetUseable()
    {
        return monsters.FirstOrDefault();
    }

    public static MonsterParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerMovement>().GetComponent<MonsterParty>();
    }
}
