using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathMenu : MonoBehaviour
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

    public void DeathMenuActivator()
    {
        _menuIsActive = true;
        StartCoroutine(DeathCorountine());
        visuals = GetComponent<UIDocument>().rootVisualElement;
        visuals.Q<VisualElement>("Background").style.visibility = Visibility.Visible;
        visuals.Q<VisualElement>("Menu").style.visibility = Visibility.Visible;
        _play.Focus();
    }

    private void OnEnable()
    {
        visuals = GetComponent<UIDocument>().rootVisualElement;
        _load = visuals.Q<Button>("Load");
        _load.clicked += LoadButton;
        _options = visuals.Q<Button>("Settings");
        _options.clicked += OptionsButton;
        _exit = visuals.Q<Button>("Exit");
        _exit.clicked += ExitButton;

        _play.Focus();

        if (_menuIsActive)
        {
            visuals.Q<VisualElement>("Background").style.visibility = Visibility.Visible;
            visuals.Q<VisualElement>("Menu").style.visibility = Visibility.Visible;
        }
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

    private IEnumerator DeathCorountine()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
    }
}
