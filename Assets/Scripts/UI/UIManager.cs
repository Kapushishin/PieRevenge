using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    private bool _isActiveMenu = false;

    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            GameState();
        }
    }

    public void GameState()
    {
        _isActiveMenu = !_isActiveMenu;

        if (_isActiveMenu)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        _menu.SetActive(_isActiveMenu);
    }
}
