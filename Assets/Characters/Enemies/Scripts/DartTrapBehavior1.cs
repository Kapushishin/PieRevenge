using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrapBehavior1 : MonoBehaviour
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        
        gameObject.SetActive(false);
    }
}
