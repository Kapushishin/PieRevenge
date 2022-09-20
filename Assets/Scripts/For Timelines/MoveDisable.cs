using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisable : MonoBehaviour
{
    private void Awake()
    {
        GameObject character = GameObject.Find("Character");
        CharacterControl controller = character.GetComponent<CharacterControl>();
        controller.CanMove = false;
        Debug.Log(controller.CanMove);
    }
}
