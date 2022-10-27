using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaStartParty : NPCBehavior
{
    [SerializeField] private GameObject _objectTrigger;

    private void Start()
    {
        Invoke("DisableObject", 0.05f);
    }
    public override void SomeAction()
    {
        EnableObject();
    }

    private void EnableObject()
    {
        _objectTrigger.SetActive(true);
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
