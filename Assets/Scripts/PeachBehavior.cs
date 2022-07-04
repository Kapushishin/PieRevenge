using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeachBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventManager.SendPeachUp();
            Destroy(gameObject);
        }
    }
}
