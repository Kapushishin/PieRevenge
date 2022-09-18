using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWriterEffect : MonoBehaviour
{
    private TextMeshProUGUI _uiText;
    public string _textToWrite;
    public int _characterIndex;
    private float _timePerCharacter;
    private float _timer;
    [SerializeField]
    private InkManager _ink;

    public void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter)
    {
        _uiText = uiText;
        _textToWrite = textToWrite;
        _timePerCharacter = timePerCharacter;
        _characterIndex = 0;
    }

    private void Update()
    {
        if (_uiText != null)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                // Display next character
                _timer += _timePerCharacter;
                _characterIndex++;
                _uiText.text = _textToWrite.Substring(0, _characterIndex);
            }

            else if (Input.GetButtonDown("Interact"))
            {
                _ink.DisplayChoices();
                _ink.CancelInvoke("DisplayChoices");
                _uiText.text = _textToWrite;
                _characterIndex = _textToWrite.Length;
            }

            if (_characterIndex >= _textToWrite.Length)
            {
                _uiText = null;
                return;
            }
        }
    }
}
