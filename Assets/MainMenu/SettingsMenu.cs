using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject mainmenu;
    [SerializeField] AudioMixer audioMixer;
    private string _windowmode = "windowed";

    // корневой элемент UI
    private static VisualElement visuals;

    // кнопки меню
    private Button _back;

    // элементы настроек
    private Slider _sound;
    private Button _volmin;
    private Button _volmax;

    private Button _windowed;
    private Button _fullscren;

    private Slider _music;
    private Button _musicvolmin;
    private Button _musicvolmax;

    private bool _settingsIsActive = false;

    private void Update()
    {
        if (Input.GetButtonDown("Menu") && _settingsIsActive)
        {
            BackButton();
        }
    }

    void OnEnable()
    {
        _settingsIsActive = true;

        visuals = GetComponent<UIDocument>().rootVisualElement;
        _back = visuals.Q<Button>("Back");
        _back.clicked += BackButton;

        _sound = visuals.Q<Slider>("Sound");
        _volmin = visuals.Q<Button>("VolMin");
        _volmin.clicked += VolMinButton;
        _volmax = visuals.Q<Button>("VolMax");
        _volmax.clicked += VolMaxButton;

        _music = visuals.Q<Slider>("Music");
        _musicvolmin = visuals.Q<Button>("MusicVolMin");
        _musicvolmin.clicked += MusicVolMinButton;
        _musicvolmax = visuals.Q<Button>("MusicVolMax");
        _musicvolmax.clicked += MusicVolMaxButton;

        _windowed = visuals.Q<Button>("Windowed");
        _windowed.clicked += WindowedButton;
        _fullscren = visuals.Q<Button>("FullScreen");
        _fullscren.clicked += FullScreenButton;

        _sound.value = PlayerPrefs.GetFloat("SoundVolume");
        _music.value = PlayerPrefs.GetFloat("MusicVolume");

        _back.Focus();
    }

    void BackButton()
    {
        PlayerPrefs.SetFloat("SoundVolume", _sound.value);
        PlayerPrefs.SetFloat("MusicVolume", _music.value);
        PlayerPrefs.SetString("WindowMode", _windowmode);
        gameObject.SetActive(false);
        mainmenu.SetActive(true);
        _settingsIsActive = false;
    }

    void VolMinButton()
    {
        _sound.value -= 5;
        audioMixer.SetFloat("Effect", _sound.value);
    }

    void VolMaxButton()
    {
        _sound.value += 5;
        audioMixer.SetFloat("Effect", _sound.value);
    }

    void MusicVolMinButton()
    {
        _music.value -= 5;
        audioMixer.SetFloat("Music", _music.value);
    }

    void MusicVolMaxButton()
    {
        _music.value += 5;
        audioMixer.SetFloat("Music", _music.value);
    }

    void WindowedButton()
    {
        Screen.fullScreen = false;
        _windowmode = "windowed";
    }

    void FullScreenButton()
    {
        Screen.fullScreen = true;
        _windowmode = "full";
    }
}
