using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { freeRoam, Battle, Menu, PartyScreen, Bag }

public class GameController : MonoBehaviour
{
    GameState state;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] battleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] public InventoryUI inventoryUI;
    [SerializeField] PartyScreen partyScreen;

    MenuController menuController;

    private void Awake()
    {
        menuController = GetComponent<MenuController>();
        MonsterDB.Init();
        ItemDB.Init();
    }
    private void Start()
    {
        playerMovement.OnEncounter += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        partyScreen.Init();
        menuController.onBack += () =>
        {
            state = GameState.freeRoam;
        };
        menuController.onMenuSelected += OnMenuSelected;
    }
    void StartBattle(GameObject obj)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerMovement.GetComponent<MonsterParty>();
        //var wildMonster = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomMonster();
        var wildMonster = obj.GetComponent<MapArea>().GetRandomMonster();

        battleSystem.StartBattle(playerParty, wildMonster);
    }
    void EndBattle(bool won)
    {
        state = GameState.freeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.freeRoam)
        {
            playerMovement.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
            //测试存取档
            //if (Input.GetKeyDown(KeyCode.N))
            //{
            //    SavingSystem.i.Save("saveSlot1");
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    SavingSystem.i.Load("saveSlot1");
            //}
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if(state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if(state == GameState.PartyScreen)
        {
            Action onSeleted = () =>
            {
                //信息
            };
            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.Menu;
            };
            partyScreen.HandleUpdate(onSeleted, onBack);
        }
        else if(state == GameState.Bag)
        {
            Action onBack = () =>
            {
                menuController.CloseMenu();
                inventoryUI.gameObject.SetActive(false);
                state = GameState.freeRoam;
            };
            inventoryUI.HandleUpdate(onBack);
        }
    }
    
    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            //状态
            partyScreen.gameObject.SetActive(true);
            partyScreen.SetPartyData(playerMovement.GetComponent<MonsterParty>().Monsters);
            state = GameState.PartyScreen;
            
        }
        else if(selectedItem ==1)
        {
            //物品
            inventoryUI.gameObject.SetActive(true);
            partyScreen.SetPartyData(playerMovement.GetComponent<MonsterParty>().Monsters);
            state = GameState.Bag;
        }
        else if(selectedItem == 2)
        {
            //保存
            SavingSystem.i.Save("saveSlot1");
            menuController.CloseMenu();
            state = GameState.freeRoam;
        }
        else if (selectedItem == 3)
        {
            //加载
            SavingSystem.i.Load("saveSlot1");
            menuController.CloseMenu();
            state = GameState.freeRoam;
        }
        else if(selectedItem == 4)
        {//回主界面
            SceneManager.LoadSceneAsync(0);
        }

        //if(Input.GetButtonDown("Back"))
        //    state = GameState.freeRoam;
    }

}
