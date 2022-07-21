using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShroom : EnemyBehavior
{
    [SerializeField] public Transform _point1;
    [SerializeField] public Transform _point2;
    private Transform _myPoint;

    [SerializeField] private GameObject _sporeAttackParticles;
    [SerializeField] private float _sporeAttackCooldown;
    [SerializeField] private float _sporeAttackTime;
    [SerializeField] private GameObject _attackingZone;

    private void Awake()
    {
        _myPoint = _point1;
    }

    protected override void Move()
    {
        base.Move();

        if (Vector2.Distance(transform.position, _target.position) > _range)
        {
            Vector3 _range = (_myPoint.position - transform.position).normalized;
            _moveDistance = _range;

            if (Vector2.Distance(transform.position, _point1.position) < 0.5f)
            {
                _myPoint = _point2;
            }
            if (Vector2.Distance(transform.position, _point2.position) < 0.5f)
            {
                _myPoint = _point1;
            }

            _rb.velocity = new Vector2(_moveDistance.x, 0f) * _speed;
        }

        else if (Vector2.Distance(transform.position, _target.position) < 1.1f)
        {
            _rb.velocity = new Vector2(0f, 0f);
        }
    }

    private IEnumerator SporeAttackCoroutine()
    {
        _animator.SetTrigger("IsAttacking");
        _rb.velocity = new Vector2(0f, 0f);
        _canAttack = false;
        _isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        //_rb.WakeUp();
        _sporeAttackParticles.SetActive(true);  
        _attackingZone.SetActive(true);
        yield return new WaitForSeconds(_sporeAttackTime);
        _isAttacking = false;
        _sporeAttackParticles.SetActive(false);
        _attackingZone.SetActive(false);
        yield return new WaitForSeconds(_sporeAttackCooldown);
        _canAttack = true;
    }

    public override void Attack()
    {
        StartCoroutine(SporeAttackCoroutine());
    }
}
