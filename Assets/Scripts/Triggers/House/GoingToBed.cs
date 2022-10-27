using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingToBed : NPCBehavior
{
    [SerializeField] private GameObject _bed;

    public override void SomeAction()
    {
        _ink.SpeedWriter = .035f;
        BedTime();
    }

    private void BedTime()
    {
        _bed.SetActive(true);
        Destroy(GameObject.Find("Grandma").GetComponent<GrandmaDialogTea>());
    }
}
