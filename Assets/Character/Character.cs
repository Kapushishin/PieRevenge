using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private bool _isGrounded;
    private float _gravityScaleDefault;
    [SerializeField]
    private float _gravityScaleFall;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gravityScaleDefault = _rb.gravityScale;
    }

    private Vector2 runVector
    {
        get
        {
            float runHorizontal = Input.GetAxis("Horizontal");

            return new Vector2(runHorizontal, 0.0f);
        }
    }

    public void Run()
    {
        _rb.velocity = runVector * speed;
    }

    public void Jump()
    {
        if (_isGrounded && Input.GetAxis("Jump") > 0)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //Invoke("ScaleGravityFall", 0.3f);
        }
        /*if (_isGrounded)
        {
            ScaleGravityDefault();
        }*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        IsGroundedUpdate(collision, true);
        ScaleGravityDefault();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        IsGroundedUpdate(collision, false);
        Invoke("ScaleGravityFall", 0.3f);
    }

    private void IsGroundedUpdate(Collision2D collision, bool value)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = value;
        }
    }

    private void ScaleGravityFall()
    {
        _rb.gravityScale = _gravityScaleFall;
    }

    private void ScaleGravityDefault()
    {
        _rb.gravityScale = _gravityScaleDefault;
    }
}
