using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject settings;

    // корневой элемент UI
    private static VisualElement visuals;

    // кнопки меню
    private Button _play;
    private Button _load;
    private Button _options;
    private Button _exit;

    private bool _menuIsActive = false;

    private InkManager _ink;
    private bool blockInteractionsLocal = false;

    private void Start()
    {
        var canvas = GameObject.Find("Canvas Dialogs");
        _ink = canvas.GetComponent<InkManager>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Menu") && !_menuIsActive)
        {
            EventManager.SendCantMove();

            blockInteractionsLocal = _ink.BlockInteractions;
            _ink.BlockInteractions = true;

            _menuIsActive = true;
            visuals = GetComponent<UIDocument>().rootVisualElement;
            visuals.Q<VisualElement>("Background").style.visibility = Visibility.Visible;
            visuals.Q<VisualElement>("Menu").style.visibility = Visibility.Visible;
            _play.Focus();
            Time.timeScale = 0.000001f;

        }

        else if (Input.GetButtonDown("Menu") && _menuIsActive)
        {
            ResumeButton();
        }
    }

    private void OnEnable()
    {
        visuals = GetComponent<UIDocument>().rootVisualElement;
        _play = visuals.Q<Button>("Resume");
        _play.clicked += ResumeButton;
        _load = visuals.Q<Button>("Load");
        _load.clicked += LoadButton;
        _options = visuals.Q<Button>("Settings");
        _options.clicked += OptionsButton;
        _exit = visuals.Q<Button>("Exit");
        _exit.clicked += ExitButton;
        
        if (_menuIsActive)
        {
            visuals.Q<VisualElement>("Background").style.visibility = Visibility.Visible;
            visuals.Q<VisualElement>("Menu").style.visibility = Visibility.Visible;
        }
    }

    private void ResumeButton()
    {
        if (blockInteractionsLocal == false)
        {
            EventManager.SendCanMove();
            _ink.BlockInteractions = false;
        }

        _menuIsActive = false;
        Time.timeScale = 1;
        GetComponent<UIDocument>().rootVisualElement.style.opacity = 100;
        visuals.Q<VisualElement>("Menu").style.visibility = Visibility.Hidden;
        visuals.Q<VisualElement>("Background").style.visibility = Visibility.Hidden;
    }

    private void LoadButton()
    {
        Time.timeScale = 1;
        GetComponent<SaveLoadSystem>().LoadGame();
        _menuIsActive = false;
    }

    private void OptionsButton()
    {
        settings.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ExitButton()
    {
        Application.Quit();
    }

    private void DisableSettingsMenu()
    {
        settings.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
