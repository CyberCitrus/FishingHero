using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryItem : MonoBehaviour
{
    Flowchart flowchart = null;
    private void Start()
    {
        new WaitForSeconds(1f);
        flowchart = FindAnyObjectByType<Flowchart>();
        if (flowchart != null)
        {
            Debug.Log("获取flowchart");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            flowchart.SendFungusMessage("道具检测");
    }
}
