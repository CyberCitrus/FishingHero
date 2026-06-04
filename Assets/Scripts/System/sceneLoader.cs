using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public const string mainMenuKey = "mainMenu";
    public const string mainSceneKey = "mainScene";

    public static void LoadAddressableScene(string key, bool loadSceneAdditively = false, bool activateOnLoad = true)
    {//通过传入的场景key加载加入资源管理的场景，默认不使用附加加载，加载完毕后立刻激活
        LoadSceneMode loadSceneMode = loadSceneAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single;
        Addressables.LoadSceneAsync(key, loadSceneMode);
    }
    public static void LoadAddressableScene(AssetReference asset, bool loadSceneAdditively = false)
    {//重载类
        LoadSceneMode loadSceneMode = loadSceneAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single;
        Addressables.LoadSceneAsync(asset, loadSceneMode);
    }
}
