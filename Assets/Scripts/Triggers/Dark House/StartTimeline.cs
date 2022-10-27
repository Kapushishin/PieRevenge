using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimeline : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _promptText;
    [SerializeField] private GameObject _timeline;

    public string PromptText => _promptText;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        _timeline.SetActive(true);
        Destroy(gameObject);
        return true;
    }
}
