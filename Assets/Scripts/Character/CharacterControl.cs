using System.Collections;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    private TrailRenderer _tr;
    private Animator _animator;
    private CharacterSounds _charSounds;

    [SerializeField] private bool isGrounded;
    private bool isFalling;

    private int playerLayerMask, platformLayerMask;

    public string tagSurface;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
        _animator = GetComponent<Animator>();
        _charSounds = GetComponent<CharacterSounds>();

        playerLayerMask = LayerMask.NameToLayer("Player");
        platformLayerMask = LayerMask.NameToLayer("Platform");
    }

    private void Update()
    {
        // чтобы не бегал во время приседа
        if (isCrouching) 
            Run(0f);
        else
            Run(speed);

        
        Dashing();
        Jump();
        Crouching();
        WallSliding();
        Attacking();
        //Landing();

        _animator.SetBool("IsJumping", isJumping);
        _animator.SetBool("IsFalling", isFalling);
        _animator.SetBool("IsWallSliding", isWallSliding);
        _animator.SetBool("IsDashing", isDashing);
        _animator.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // Проверка на землю и платформы
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                // берем таг поверхности, на которой стоим
                tagSurface = colliders[i].gameObject.tag.ToString();
                isGrounded = true;
                //isJumping = false;
                isFalling = false;
                isWallSliding = false;
            }
        }

        // если скорость меньше нуля, то включается анимация падения
        if (_rb.velocity.y < 0 && !isWallSliding && !isGrounded)
        {
            isFalling = true;
            isJumping = false;

        }
        else if (_rb.velocity.y > 0 && !isGrounded)
        {
            isJumping = true;
            isFalling = false;
        }
            
    }

    #region Movement
    [Header("Movement")]

    [SerializeField] private float speed;
    private float runHorizontal;
    private bool isFacingRight;

    private void Run(float speed)
    {
        // когда персонаж находится в дэше, механика бега не будет его перебивать
        if (isDashing) 
            return;

        runHorizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(runHorizontal * speed, _rb.velocity.y);;
        _animator.SetFloat("Speed", Mathf.Abs(runHorizontal));

        // Поворот по направлению движения
        if (isFacingRight && runHorizontal > 0f || !isFacingRight && runHorizontal < 0f)
        {
            Vector3 _localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
            _animator.Play("Player1_Turnaround");
        }
    }
    #endregion

    #region Crouching

    [Header("Crouch")]

    private bool isCrouching;
    private void Crouching() 
    {
        // Анимация приседания
        if (Input.GetButtonDown("Crouch"))
        {
            if (isGrounded)
            {
                _animator.SetBool("IsCrouching", true);
                _animator.Play("Player1_Crouch");
                isCrouching = true;
            }
        }

        //прыжки в приоритете
        if (isJumpingOff) 
            return;

        else if (Input.GetButtonUp("Crouch"))
        {
            _animator.SetBool("IsCrouching", false);
            _animator.Play("Player1_Get_up");
            isCrouching = false;
        }
    }

    #endregion

    #region Jumping
    [Header("Jump")]

    [SerializeField] private float jumpForce;
    //время, которое дается на то, что бы нажать прыжок после того как сошел с платформы
    private float coyoteTime = 0.2f; 
    private float coyoteTimeCounter;
    //буфер прыжка, который позволяет прыгнуть, когда персонаж еще не совсем приземлился
    private float jumpBufferTime = 0.2f; 
    private float jumpBufferCounter;
    private float jumpCooldown = 0.4f;
    private bool isJumping;
    private bool isJumpingOff;
    private bool canDoubleJump = false;

    private float ignoreLayerTime = 0.3f;
    private float cantCrouchingJump = 0.5f;

    private void Jump()
    {
        if (SwitchParametres.CanJump == true)
        {
            if (isGrounded && !isCrouching)
            {
                // Счетчик, пока на земле, равен отведенному времени на прыжок койота
                coyoteTimeCounter = coyoteTime;

                if (SwitchParametres.CanDoubleJump == true)
                {
                    canDoubleJump = false;
                }
                isJumping = false;
                isJumpingOff = false;
            }
            else
            {
                // Если игрок в воздухе, то значение счетчика уменьшается на 1 кадр до нуля, пока возможность прыгнуть не станет невозможной
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump"))
            {
                // Как только нажали на прыжок, счетчик становится равен отведенному времени на буфер прыжка
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                // Пока мы не нажимаем прыжок, уменьшается счетчик на 1 кадр
                jumpBufferCounter -= Time.deltaTime;
            }

            if ((coyoteTimeCounter > 0f && jumpBufferCounter > 0f ||
                Input.GetButtonDown("Jump") && canDoubleJump && !isGrounded) && !isCrouching) // прыжок
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                // для прыжков в воздухе, счетчик сбрасывается
                jumpBufferCounter = 0f;
                canDoubleJump = false;
                isJumping = true;
                //StartCoroutine(JumpCooldown());
            }

            // высокий прыжок
            if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f && !isCrouching)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
                // Счетчик сбрасывается, как только отпускается кнопка прыжка
                coyoteTimeCounter = 0f;
            }

            // Спрыгивание с платформы, после приседания
            if (isCrouching && Input.GetButtonDown("Jump") && isGrounded)
            {
                isJumpingOff = true;
                // Отключение колизий платформ и игрока
                Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, true);
                // Включение колизий платформ и игрока
                Invoke("IgnoreLayerOff", ignoreLayerTime);
                // Запрет на спрыгивание на некоторое время
                Invoke("CantCrouchingJump", cantCrouchingJump);
            }
        }
    }
    // если очень быстро жать пробел, то делает двойной прыжок, так не надо
    private IEnumerator JumpCooldown() 
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }
    private void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, false);
    }
    private void CantCrouchingJump()
    {
        isCrouching = false;
    }

    #endregion

    #region Dashing
    [Header("Dash")]

    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;

    private void Dashing()
    {
        // рывок
        if (SwitchParametres.CanDash == true)
        {
            if (Input.GetButtonDown("Dash") && canDash)
            {
                StartCoroutine(DashCoroutine());
            }
        }

        if (isDashing)
        {
            //берем направление персонажа по х * силу рывка
            _rb.velocity = new Vector2(_rb.transform.localScale.x * dashingForce, 0f); 
        }
    }
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rb.gravityScale;
        // отключаем гравитацию, чтобы персонаж не двигался наискосок
        _rb.gravityScale = 0f; 
        _tr.emitting = true;
        _charSounds.PlayDashSound();
        yield return new WaitForSeconds(dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion

    #region Walls sliding
    [Header("Wall slide")]

    [SerializeField] private float speedWallSliding = -1f;
    private bool isWallSliding = false;
    [SerializeField] private float distanceToWall;

    private void WallSliding()
    {
        // Скольжение по стенам
        // Проверка горизонтальным лучом заданной длины, есть ли перед игроком стена
        // Если да, то замедлять скорость падения
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distanceToWall);
        if (hit.collider != null)
        {
            if (!isGrounded && _rb.velocity.y < 0.01f)
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    _rb.velocity = new Vector2(0, speedWallSliding);
                    isWallSliding = true;
                    
                }
                else
                {
                    isWallSliding = false;
                }
            }
        }
    }
    #endregion

    /*[SerializeField] private float distanceToGround;
    private void Landing()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround);
        if (hit.collider != null)
        {
            if (hit.distance > 1.5f)
            {
                _animator.Play("Player1_Landing");
                Debug.Log("landing");
            }
        }
    }*/

    #region Attack
    [Header("Attack")]

    [SerializeField] private Transform attackCheck;
    [SerializeField] private LayerMask attackLayer;
    private bool canAttack = false;
    private bool isAttacking = false;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackTime;
    // Для разной анимации атаки, чтобы делать последовательное комбо
    private int attackAnimCondition = 0;
    [SerializeField] private float damage;
    [SerializeField] private float radiusAttack;
    IDamageToEnemy target;

    private void Attacking()
    {
        Collider2D[] attackColliders = Physics2D.OverlapCircleAll(attackCheck.position, radiusAttack, attackLayer);

        if (Input.GetButtonDown("Attack") && canAttack)
        {

            if (attackColliders.Length > 0 && canAttack)
            {
                target = attackColliders[0].GetComponent<IDamageToEnemy>();
                target.EnemyGetDamaged(this, damage);
            }

            else
            {
                if (target != null)
                {
                    target = null;
                }
            }

            StartCoroutine(AttackCoroutine());

            if (attackAnimCondition == 0)
            {
                _animator.Play("Player1_Attack1");
                attackAnimCondition++;
            }
            else if (attackAnimCondition == 1)
            {
                _animator.Play("Player1_Attack2");
                attackAnimCondition++;
            }
            else if (attackAnimCondition == 2)
            {
                _animator.Play("Player1_Attack3");
                attackAnimCondition = 0;
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    #endregion
}
