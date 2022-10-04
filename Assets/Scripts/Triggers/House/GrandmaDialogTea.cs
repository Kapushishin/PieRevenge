using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaDialogTea : NPCBehavior
{
    [SerializeField] private GameObject _teaTimeTrigger;
    private void OnEnable()
    {
        EventManager.OnEndDialog += EnableTeaTimeTrigger;
    }

    private void OnDisable()
    {
        EventManager.OnEndDialog -= EnableTeaTimeTrigger;
    }

    public override void SomeAction()
    {
    }

    private void EnableTeaTimeTrigger()
    {
        _teaTimeTrigger.SetActive(true);
    }
}
