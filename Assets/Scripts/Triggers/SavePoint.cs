using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint :  NPCBehavior
{
    public override void SomeAction()
    {
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<SaveLoadSystem>().SaveGame();
        GetComponentInChildren<AudioSource>().Play();
    }
}
