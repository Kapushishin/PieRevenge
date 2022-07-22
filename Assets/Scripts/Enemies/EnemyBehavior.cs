using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour, IDamageToEnemy
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    protected bool _isGrounded;

    [SerializeField] protected float _speed;
    [SerializeField] protected float _rangeToChasing;
    [SerializeField] protected float _rangeToSrandingNearPlayer;

    [SerializeField] public float _health;

    private bool _isFacingRight;
    protected Vector2 _moveDistance;

    protected Rigidbody2D _rb;
    protected Transform _target;
    protected Animator _animator;

    protected bool _canAttack = true;
    protected bool _isAttacking = false;

    protected void Start()
    {
        // Получение положения игрока
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        // Проверка на землю
        _isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isGrounded = true;
            }
        }

        // Поворот объекта по направлению движения
        if (_isFacingRight && _moveDistance.x > 0f || !_isFacingRight && _moveDistance.x < 0f)
        {
            Vector3 _localScale = transform.localScale;
            _isFacingRight = !_isFacingRight;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
        }

        _animator.SetFloat("Speed", Mathf.Abs(_moveDistance.x));

    }

    private void FixedUpdate()
    {

        if (!_isAttacking)
        {
            Move();
        }

        if (_canAttack)
        {
            Attack();
        }

    }

    protected virtual void Move()
    {
        // Когда дистанция от врага до игрока станет меньше заданного значения, враг побежит за игроком
        if (Vector2.Distance(transform.position, _target.position) < _rangeToChasing && 
            Vector2.Distance(transform.position, _target.position) > _rangeToSrandingNearPlayer)
        {
            Vector3 _distance = (_target.position - transform.position).normalized;
            _moveDistance = _distance;
            _rb.velocity = new Vector2(_moveDistance.x, 0f) * _speed;
        }

        // Если расстояние до игрока меньше заданного значения,
        // То враг встанет. Сделано, чтобы враг не толкал рб игрока.
        else if (Vector2.Distance(transform.position, _target.position) < _rangeToSrandingNearPlayer)
        {
            _rb.velocity = new Vector2(0f, 0f);
        }
    }

    // Нужно реализовать поведение во время атаки в дочернем скрипте врага
    public abstract void Attack();

    // Реализация интерфейса IDamaging. Поведение при получении урона от игрока
    public void EnemyGetDamaged(CharacterControl player, float damage)
    {
        Debug.Log(_health);
        if (_health > 0)
        {
            _health -= damage;
            //_animator.SetTrigger("IsHurt");
            //StartCoroutine(Stuned());
            if (_health == 0)
            {
                //isDead = true;
                //_animator.SetTrigger("IsDead");
                //Instantiate(deathParticles, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }
    }
}
