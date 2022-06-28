using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrapBehavior : DamageBehavior
{
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed);
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // чтобы такой же метод в DamageBehavior работал в первую очередь
        Destroy(gameObject);
        Debug.Log("hit");
    }
}
