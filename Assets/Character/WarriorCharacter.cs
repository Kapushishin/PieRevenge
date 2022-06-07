using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCharacter : Character
{
    private void Update()
    {
        Jump();
        HighJump();
        DoubleJump();
        Flip();
    }

    private void FixedUpdate()
    {
        Run();
    }
}
