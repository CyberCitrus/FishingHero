using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] public GameObject menu;

    public event Action<int> onMenuSelected;
    public event Action onBack;

    List<TextMeshProUGUI> menuItems;
    int selectedItem = 0;

    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<TextMeshProUGUI>().ToList();
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        UpdateItemSelection();
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void HandleUpdate()
    {
        int preSelection = selectedItem;
        if (Input.GetButtonDown("Down"))
        {
            ++selectedItem;
        }
        else if (Input.GetButtonDown("Up"))
        {
            --selectedItem;
        }
        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count-1);
        if (preSelection != selectedItem)
            UpdateItemSelection();

        if (Input.GetButtonDown("Confirm"))
        {
            onMenuSelected?.Invoke(selectedItem);
        }
        else if (Input.GetButtonDown("Back"))
        {
            onBack?.Invoke();
            CloseMenu();
        }
    }
    void UpdateItemSelection()
    {
        for(int i = 0; i < menuItems.Count; i++) 
        {
            if (i == selectedItem)
            {
                menuItems[i].color = Color.yellow;
            }
            else
            {
                menuItems[i].color = Color.white;
            }
        }
    }
}
