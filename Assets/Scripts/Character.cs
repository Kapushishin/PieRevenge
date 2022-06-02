using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField]
    private float speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private Vector2 runVector
    {
        get
        {
            float runHorizontal = Input.GetAxis("Horizontal");
            float runVertical = Input.GetAxis("Vertical");

            return new Vector2(runHorizontal, runVertical);
        }
    }
    public void Run()
    {
        _rb.velocity = runVector * speed;
    }

    public void Jump()
    {
        Debug.Log("jump");
    }
}
