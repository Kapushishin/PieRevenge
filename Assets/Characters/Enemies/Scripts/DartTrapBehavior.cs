using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrapBehavior : DamageBehavior
{
    [SerializeField] private float _speed;

    public void ActivateDart()
    {
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        transform.Translate(0, _speed, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        gameObject.SetActive(false);
    }
}
