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
	public Story _story;
    private TextAsset _newStory;

	[SerializeField]
	private TextMeshProUGUI _textField = null;
    [SerializeField]
    private VerticalLayoutGroup _choiceButtonContainer;
    [SerializeField]
    private Button _choiceButtonPrefab;

    public bool _blockInteractions = false;

    [SerializeField]
    public TextMeshProUGUI _nameField = null;

    [SerializeField]
    private TextWriterEffect _textWriter;

    //ƒобавление текта из NPC, запуск диалога
    public void NewStory(TextAsset text)
    {
        _newStory = text;
        StartStory(_newStory);
    }

    // Ќачало диалога, отображение первой строчки
    private void StartStory(TextAsset text)
    {
        _story = new Story(text.text);
        DisplayNextLine();
    }

    // ќтображение следующей строки
    public void DisplayNextLine()
    {
        if (_story.canContinue)
        {
            string text = _story.Continue();
            // удаление лишних пробелов, если есть
            text = text?.Trim();
            _textWriter.AddWriter(_textField, text, .1f);
        }

        if (_story.currentChoices.Count > 0)
        {
            Invoke("DisplayChoices", _textWriter._textToWrite.Length / 10f);
        }

        else
        {
            _textField.text = null;
            // если конец диалога, то закрыть канвас
            gameObject.SetActive(false);
            // заблокировать возможность бесконечно открывать диалог заново внутри диалога
            _blockInteractions = false;
        }
    }

    // ѕоказать выбор ответов
    public void DisplayChoices()
    {
        // перебор всех вариантов ответа
        for (int i = 0; i < _story.currentChoices.Count; i++)
        {
            Choice choice = _story.currentChoices[i];
            // создание кнопки с ответом
            Button button = CreateChoiceButton(choice.text.Trim());
            // говорим кнопке, что делать, если на нее нажали
            button.onClick.AddListener(() => OnClickChoiceButton(choice));
        }
    }

    // —оздание кнопки, показывающие вариант ответа
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

    //  огда нажимаем на кнопку ответа, говорим инку, что конкретно выбрали
    private void OnClickChoiceButton(Choice choice)
    {
        _story.ChooseChoiceIndex(choice.index);
        // убираем варианты ответа с экрана
        RefreshChoiceView();
        _story.Continue();
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
