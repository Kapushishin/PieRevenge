using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShroom : EnemyBehavior
{
    protected override float Speed => 3f;
    protected override float Range => 5f;

    [SerializeField] public Transform _point1;
    [SerializeField] public Transform _point2;
    private Transform _myPoint;

    private void Awake()
    {
        _myPoint = _point1;
    }

    protected override void Move()
    {
        base.Move();
        if (Vector2.Distance(transform.position, _target.position) > Range)
        {
            if (Vector2.Distance(transform.position, _point1.position) < 0.21f)
            {
                _myPoint = _point2;
            }
            if (Vector2.Distance(transform.position, _point2.position) < 0.21f)
            {
                _myPoint = _point1;
            }

            Vector3 _range = (_myPoint.position - transform.position).normalized;
            _moveRange = _range;

            _rb.velocity = new Vector2(_moveRange.x, 0f) * Speed;
            
        }
    }
}
