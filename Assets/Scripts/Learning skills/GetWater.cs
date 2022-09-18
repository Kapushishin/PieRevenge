using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWater : NPCBehavior
{
    [SerializeField] private AudioSource _waterSound;

    public override void SomeAction()
    {
        SwitchParametres.SwitchHaveWater();
        SwitchParametres.SwitchGoHome();
        _waterSound.Play();
    }
}
