using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonKeyboard : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            _button.onClick.Invoke();
        }
    }
}
