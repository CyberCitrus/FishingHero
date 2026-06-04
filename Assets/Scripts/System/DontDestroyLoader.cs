using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyLoader : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyPrefab;
    public void Awake()
    {
        var existingObjects = FindObjectsOfType<DontDestroy>();
        if(existingObjects.Length == 0)
        {
            Instantiate(dontDestroyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
