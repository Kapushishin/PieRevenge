using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkGrandma : NPCBehavior
{
    [SerializeField] private GameObject _timeline;

    private void Start()
    {
        EventManager.OnEndDialog += StartTimeline;
    }

    private void OnDisable()
    {
        EventManager.OnEndDialog -= StartTimeline;
    }

    public override void SomeAction()
    {
        
    }

    private void StartTimeline()
    {
        _timeline.SetActive(true);
    }
}
