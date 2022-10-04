using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivation : NPCBehavior
{
    private void OnEnable()
    {
        _ink.SpeedWriter = .035f;
    }

    private void OnDisable()
    {
        _ink.SpeedWriter = .1f;
    }

    public override void SomeAction()
    {
    }
}
