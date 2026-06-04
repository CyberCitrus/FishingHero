using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Move", menuName ="Create new move")]
public class moveBase : ScriptableObject
{

    [SerializeField] string name;
    [TextArea] 
    [SerializeField]string info;

    [SerializeField] int power;
    [SerializeField] int point;


    public string Name { get { return name; } }
    public int Power { get { return power; } }
    public int Point { get { return point; } }
    public string Info { get { return info; } }
}