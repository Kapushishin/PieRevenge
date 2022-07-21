using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehavior : MonoBehaviour
{
    [SerializeField] protected float _damage;

    private IDamaging _target;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _target = collision.GetComponent<IDamaging>();
            _target.GetDamaged(this, _damage);
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("pupupu");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("hes here!");
            _target = collision.GetComponent<IDamaging>();
            _target.GetDamaged(this, _damage);
        }
    }


}
