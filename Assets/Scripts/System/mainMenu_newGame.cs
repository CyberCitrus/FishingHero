using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class mainMenu_newGame : MonoBehaviour
{
    public void newGame()
    {
        //sceneLoader.LoadAddressableScene(newGameScene);
        SceneManager.LoadSceneAsync(3);
    }
    public void continueGame()
    {
        SceneManager.LoadSceneAsync(7);

    }

    public void closeGame()
    {
        Application.Quit();
    }
}
