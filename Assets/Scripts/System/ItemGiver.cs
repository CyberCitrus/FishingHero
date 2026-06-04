using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] ItemBase item;

    public IEnumerator GiveItem(PlayerMovement player)
    {
        player.GetComponent<Inventory>().AddItem(item);
        yield return null;
    }
}
