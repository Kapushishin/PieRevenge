using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTeaTime : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _textPrompt;
    [SerializeField] private GameObject _timeLineTeaTime;

    public string PromptText => _textPrompt;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        _timeLineTeaTime.SetActive(true);
        Destroy(gameObject);
        return true;
    }
}
