using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsBehavior : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthBehavior>().GetDamaged(damage);
        }
            
    }
}
