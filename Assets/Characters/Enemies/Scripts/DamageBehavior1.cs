using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehavior1 : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<HealthBehavior>().GetDamaged(damage);   
    }
}
