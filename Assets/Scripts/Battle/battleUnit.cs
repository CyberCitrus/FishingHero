using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class battleUnit : MonoBehaviour
{
    [SerializeField] public bool isPlayer;
    Image image;
    public Monster monster { get; set; }

    private void Awake()
    {
        if (!isPlayer)
        {
            image = GetComponent<Image>();
        }
    }
    public void Setup(Monster choose)
    {
        monster = choose;
        if (!isPlayer)
        {
            image.sprite = monster.Base.Sprite;
        }
    }
}
