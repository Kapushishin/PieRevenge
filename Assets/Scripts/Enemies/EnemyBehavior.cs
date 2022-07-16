using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    protected abstract float Speed { get; }
    protected abstract float Range { get; }

    private bool _isFacingRight;
    protected Vector2 _moveRange;

    protected Rigidbody2D _rb;
    protected Transform _target;
    protected Animator _animator;

    protected virtual void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isFacingRight && _moveRange.x > 0f || !_isFacingRight && _moveRange.x < 0f)
        {
            Vector3 _localScale = transform.localScale;
            _isFacingRight = !_isFacingRight;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
        }
        _animator.SetFloat("Speed", Mathf.Abs(_moveRange.x));

    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (Vector2.Distance(transform.position, _target.position) < Range)
        {
            Vector3 _range = (_target.position - transform.position).normalized;
            _moveRange = _range;
            _rb.velocity = new Vector2(_moveRange.x, 0f) * Speed;
        }
    }
}
