using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCharacter : Character
{
    private void Update()
    {
        Run();
        Jump();
        Debug.Log($"coord: {transform.position.y}");
    }
}
