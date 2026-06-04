using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class changeToCave : MonoBehaviour
{
    public GameObject UI;
    [SerializeField] AssetReference battleArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UI.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        UI.SetActive(false);
    }
}
