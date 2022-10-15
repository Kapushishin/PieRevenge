using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShroom : EnemyBehavior
{
    public override void Move()
    {
        base.Move();
        _rb.velocity = new Vector2(_moveDistance.x, 0f);
    }

    public override void Attack()
    {
        // он атакует своим телом))
    }

    public override void Dead()
    {
        base.Dead();
        _rb.velocity = new Vector2(0f, 0f);
    }
}
