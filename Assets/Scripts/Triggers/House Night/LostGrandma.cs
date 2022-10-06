using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostGrandma : NPCBehavior
{
    [SerializeField] private GameObject _exitDoor;
    public override void SomeAction()
    {
        _exitDoor.SetActive(true);
    }
}
