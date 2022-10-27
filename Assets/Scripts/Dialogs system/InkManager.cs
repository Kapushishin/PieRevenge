using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections;
using TMPro;

public class InkManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

	[SerializeField]
	public Story Story;
    private TextAsset _newStory;

	[SerializeField]
	private TextMeshProUGUI _textField = null;
    [SerializeField]
    private VerticalLayoutGroup _choiceButtonContainer;
    [SerializeField]
    private Button _choiceButtonPrefab;

    public bool BlockInteractions = false;

    [SerializeField]
    public TextMeshProUGUI NameField = null;

    [SerializeField]
    private TextWriterEffect _textWriter;
    [SerializeField]
    public float SpeedWriter = .1f;

    [SerializeField] private AudioSource _writingSFX;

    private void Awake()
    {
        Invoke("DisableCanvas", .05f);
    }

    private void DisableCanvas()
    {
        _canvas.SetActive(false);
    }

    //Добавление текта из NPC, запуск диалога
    public void NewStory(TextAsset text)
    {
        _newStory = text;
        StartStory(_newStory);
    }

    // Начало диалога, отображение первой строчки
    private void StartStory(TextAsset text)
    {
        Story = new Story(text.text);
        DisplayNextLine();
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>().CanMove = false;
    }

    // Отображение следующей строки
    public void DisplayNextLine()
    {
        if (Story.canContinue)
        {
            string text = Story.Continue();
            // удаление лишних пробелов, если есть
            text = text?.Trim();
            _textWriter.AddWriter(_textField, text, SpeedWriter);
        }

        if (Story.currentChoices.Count > 0)
        {
            _writingSFX.pitch = UnityEngine.Random.Range(.4f, 1f);
            _writingSFX.Play();
            Invoke("DisplayChoices", _textWriter._textToWrite.Length / 10f);
        }

        else
        {
            _textField.text = null;
            // если конец диалога, то закрыть канвас
            _canvas.SetActive(false);
            // разблокировать возможность открывать диалог
            BlockInteractions = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>().CanMove = true;
            // выключить непися если надо
            EventManager.SendDestroyNPC();
            SpeedWriter = .1f;
        }
    }

    // Показать выбор ответов
    public void DisplayChoices()
    {
        _writingSFX.Stop();
        // перебор всех вариантов ответа
        for (int i = 0; i < Story.currentChoices.Count; i++)
        {
            Choice choice = Story.currentChoices[i];
            // создание кнопки с ответом
            Button button = CreateChoiceButton(choice.text.Trim());
            // говорим кнопке, что делать, если на нее нажали
            button.onClick.AddListener(() => OnClickChoiceButton(choice));
        }
    }

    // Создание кнопки, показывающие вариант ответа
    Button CreateChoiceButton(string text)
    {
        //создание кнопки выбора из префаба
        Button choiceButton = Instantiate(_choiceButtonPrefab);
        choiceButton.transform.SetParent(_choiceButtonContainer.transform, false);

        //изменение текста на кнопке из префаба
        TextMeshProUGUI buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        return choiceButton;
    }

    // Когда нажимаем на кнопку ответа, говорим инку, что конкретно выбрали
    private void OnClickChoiceButton(Choice choice)
    {
        Story.ChooseChoiceIndex(choice.index);
        // убираем варианты ответа с экрана
        RefreshChoiceView();
        Story.Continue();
        DisplayNextLine();
    }

    // убираем варианты ответа с экрана
    private void RefreshChoiceView()
    {
        if (_choiceButtonContainer != null)
        {
            foreach (Button button in _choiceButtonContainer.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }
    }
}
