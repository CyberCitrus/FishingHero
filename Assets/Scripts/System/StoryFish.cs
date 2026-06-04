using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StoryFish : MonoBehaviour
{
    Flowchart flowchart = null;
    Collider2D col;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        new WaitForSeconds(1f);
        flowchart = FindAnyObjectByType<Flowchart>();
        if (flowchart != null)
        {
            Debug.Log("µöÓãÄ£¿é»ñÈ¡flowchart");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            flowchart.SendFungusMessage("µöÓã");
        }
    }
}
