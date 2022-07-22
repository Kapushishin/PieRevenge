using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBehavior : MonoBehaviour
{
    [SerializeField] protected float _damage;

    private IDamageToPlayer _target;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _target = collision.GetComponent<IDamageToPlayer>();
            _target.GetDamaged(this, _damage);
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
