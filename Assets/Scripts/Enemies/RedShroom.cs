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
        // Начальная точка, куда пойдет патрулировать.
        _myPoint = _point1;
    }

    protected override void Move()
    {
        base.Move();

        // Если дистанция до игрока больше заданного значения,
        if (Vector2.Distance(transform.position, _target.position) > _rangeToChasing)
        {
            // То враг будет патрулировать между 2 точками:
            // Отмеряя дистанцию до выбранной точки. Если она меньше какого-то значения,
            // То менять цель на вторую точку и идти к ней. И так бесконечно.
            if (Vector2.Distance(transform.position, _point1.position) < 0.5f)
            {
                _myPoint = _point2;
            }
            if (Vector2.Distance(transform.position, _point2.position) < 0.5f)
            {
                _myPoint = _point1;
            }
            // Дистанцию до точки привести к значению близкому к 1.
            // Оно может быть положительным и отрицательным.
            // Таким образом задается направление движения по оси х.
            Vector3 _range = (_myPoint.position - transform.position).normalized;
            _moveDistance = _range;
            // И умножается на скорость.
            _rb.velocity = new Vector2(_moveDistance.x, 0f) * _speed;
        }
    }

    // Атака
    private IEnumerator SporeAttackCoroutine()
    {
        _animator.SetTrigger("IsAttacking");
        _rb.velocity = new Vector2(0f, 0f);
        _canAttack = false;
        _isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        // РБ засыпает из-за низкой скорости, приходится будит перед атакой, иначе урон не наносится
        _rb.WakeUp();
        _sporeAttackParticles.SetActive(true);
        // Коллайдер зоны атаки включиться во время того, как разлетаются споры.
        _attackingZone.SetActive(true);
        yield return new WaitForSeconds(_sporeAttackTime);
        _isAttacking = false;
        _sporeAttackParticles.SetActive(false);
        _attackingZone.SetActive(false);
        yield return new WaitForSeconds(_sporeAttackCooldown);
        _canAttack = true;
    }

    // Реализация атаки
    public override void Attack()
    {
        StartCoroutine(SporeAttackCoroutine());
    }
}
