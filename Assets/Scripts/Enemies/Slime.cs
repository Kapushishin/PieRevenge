using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyBehavior
{
    public override void Move()
    {
        base.Move();

        if (Vector2.Distance(transform.position, _target.position) < _rangeToChasing && _canAttack)
        {
            Motion(_target);
        }

        //_rb.velocity = new Vector2(_moveDistance.x, 0f);
        _rb.velocity = _moveDistance;
    }

    public override void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    public override void Dead()
    {
        base.Dead();
        _rb.velocity = new Vector2(0f, 0f);
    }

    private IEnumerator AttackCoroutine()
    {
        _canAttack = false;
        _isAttacking = true;
        _rb.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(_attackCooldown);
        _isAttacking = false;
        _canAttack = true;
    }
}
