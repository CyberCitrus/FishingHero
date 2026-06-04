using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class clickToCave : MonoBehaviour
{
    int MAINAREA = 1;
    int BATTLEAREA = 7;
    Vector3 battle = new Vector3(-7, -2, -10);
    Vector3 main = new Vector3(30, -1, -1);
    Fader fader;
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        fader = FindObjectOfType<Fader>();
    }

    public void ToBattleInit()
    {
        SceneManager.LoadSceneAsync(BATTLEAREA);
    }
    public void ToBattle()
    {
        player.transform.position = battle;
    }
    public void ToMain()
    {
        //SceneManager.LoadSceneAsync(MAINAREA);
        player.transform.position = main;
    }
    IEnumerator click()
    {
        //sceneLoader.LoadAddressableScene(battleArea);
        yield return fader.FadeIn(0.5f);
        yield return SceneManager.LoadSceneAsync(BATTLEAREA);
        yield return fader.FadeOut(0.5f);
    }
    IEnumerator toMain()
    {
        //sceneLoader.LoadAddressableScene(mainArea);
        yield return fader.FadeIn(0.5f);
        yield return SceneManager.LoadSceneAsync(MAINAREA);
        yield return fader.FadeOut(0.5f);
    }
}
