using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetJumpLearning : NPCBehavior
{
    [SerializeField] private AudioSource _pickUpSound;
    public override void SomeAction()
    {
        SwitchParametres.CanJump = true;
        _pickUpSound.Play();
    }
}
