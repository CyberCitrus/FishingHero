using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    public bool isLoaded { get; private set; }
    private void Start()
    {
        {
            SceneManager.LoadSceneAsync(1);
            SceneManager.LoadSceneAsync(2,LoadSceneMode.Additive);
            LoadSceneRandom();
            isLoaded = true;
        }
    }

    public void LoadSceneRandom()
    {
        {
            SceneManager.LoadSceneAsync(Random.Range(4, 7), LoadSceneMode.Additive);
            isLoaded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag == "Player")
        //{
        //  Debug.Log($"½øÈë{gameObject.name}");
        //if(gameObject.name == "battleArea" && !isLoaded)
        //{
        //    SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
        //}
        //else LoadSceneRandom();
        //}
    }
}
