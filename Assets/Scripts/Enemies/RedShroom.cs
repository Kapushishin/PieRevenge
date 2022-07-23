using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShroom : EnemyBehavior
{
    [SerializeField] private GameObject _sporeAttackParticles;
    [SerializeField] private float _sporeAttackCooldown;
    [SerializeField] private float _sporeAttackTime;
    [SerializeField] private GameObject _attackingZone;

    public override void Move()
    {
        base.Move();
        // Когда дистанция от врага до игрока станет меньше заданного значения, враг побежит за игроком
        if (Vector2.Distance(transform.position, _target.position) < _rangeToChasing &&
            Vector2.Distance(transform.position, _target.position) > _rangeToSrandingNearPlayer)
        {
            Vector3 _distance = (_target.position - transform.position).normalized;
            _moveDistance = _distance;
            _rb.velocity = new Vector2(_moveDistance.x, 0f) * _speed;
        }

        // Если расстояние до игрока меньше заданного значения,
        // То враг встанет. Сделано, чтобы враг не толкал рб игрока.
        else if (Vector2.Distance(transform.position, _target.position) < _rangeToSrandingNearPlayer)
        {
            _rb.velocity = new Vector2(0f, 0f);
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
