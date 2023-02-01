using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBehavior : MonoBehaviour
{
    [SerializeField] protected float _damage;
    private Animator _animator;

    private IDamageToPlayer _target;

    private void Start()
    {
        if (GetComponent<Animator>())
            _animator = GetComponent<Animator>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GetComponent<EnemyBehavior>())
            {
                GetComponent<EnemyBehavior>()._isAttacking = true;
            }
            
            _target = collision.GetComponent<IDamageToPlayer>();
            _target.GetDamaged(this, _damage);

            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _target = collision.GetComponent<IDamageToPlayer>();
            _target.GetDamaged(this, _damage);
        }
    }


}
