using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWater : NPCBehavior
{
    [SerializeField] private AudioSource _waterSound;
    [SerializeField] private GameObject _door;

    public override void SomeAction()
    {
        _door.SetActive(true);
        _waterSound.Play();
    }
}
