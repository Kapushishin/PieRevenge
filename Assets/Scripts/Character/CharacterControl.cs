using System.Collections;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    private Animator _animator;
    private CharacterSounds _charSounds;

    [SerializeField] private bool isGrounded;
    private bool isFalling;

    private int playerLayerMask, platformLayerMask;

    public string tagSurface;

    public bool CanMove = true;

    protected GameObject _canvas;
    protected InkManager _ink;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _charSounds = GetComponent<CharacterSounds>();

        playerLayerMask = LayerMask.NameToLayer("Player");
        platformLayerMask = LayerMask.NameToLayer("Platform");

        _canvas = GameObject.Find("Canvas Dialogs");
        _ink = _canvas.gameObject.GetComponent<InkManager>();

        EventManager.OnCanMove += CanMoveTrue;
        EventManager.OnCantMove += CanMoveFalse;
    }

    private void OnDisable()
    {
        EventManager.OnCanMove -= CanMoveTrue;
        EventManager.OnCantMove -= CanMoveFalse;
    }

    private void Update()
    {
        if (CanMove)
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

        if (_ink.BlockInteractions)
        {
            _animator.SetFloat("Speed", 0f);
            _animator.SetBool("IsInteractionsBlocked", true);
        }
        else _animator.SetBool("IsInteractionsBlocked", false);
    }

    private void FixedUpdate()
    {
        // Проверка на землю и платформы
        isGrounded = false;
        isWallSliding = false;

        Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f, groundLayer);

        for (int i = 0; i < collidersGround.Length; i++)
        {
            if (collidersGround[i].gameObject != gameObject)
            {
                // берем таг поверхности, на которой стоим
                tagSurface = collidersGround[i].gameObject.tag.ToString();
                isGrounded = true;
                //isJumping = false;
                isFalling = false;
                isWallSliding = false;
                isJumping = false;
            }
        }

        Collider2D[] collidersWall = Physics2D.OverlapCircleAll(wallsCheck.position, 0.15f, wallLayer);

        for (int i = 0; i < collidersWall.Length; i++)
        {
            if (collidersWall[i].gameObject != gameObject)
            {
                isWallSliding = true;
                Debug.Log(collidersWall[i].gameObject);
                //WallSliding();
            }
        }

        // если скорость меньше нуля, то включается анимация падения
        if (_rb.velocity.y < 0 && !isWallSliding && !isGrounded)
        {
            isFalling = true;
            isJumping = false;

        }
        else if (_rb.velocity.y > 0 && isWallSliding && !isGrounded)
        {
            isJumping = true;
            isFalling = false;
        }
            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, .1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, radiusAttack);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallsCheck.position, .15f);
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
    private float ignoreLayerTime = 0.3f;
    private float cantCrouchingJump = 0.5f;
    private bool canDoubleJump;
    [SerializeField] public bool isCanJump;
    [SerializeField] public bool _isCanDoubleJump;

    private void Jump()
    {
        if (SwitchParametres.CanJump)
        {
            if (isGrounded && !isCrouching)
            {
                // Счетчик, пока на земле, равен отведенному времени на прыжок койота
                coyoteTimeCounter = coyoteTime;

                if (SwitchParametres.CanDoubleJump == true)
                {
                    canDoubleJump = true;
                }

                //isJumping = false;
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

                if (_rb.velocity.y > 0.01f)
                {
                    isJumping = true;
                }

                StartCoroutine(JumpCooldown());
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

    [SerializeField] public bool isCanDash;

    private void Dashing()
    {
        // рывок
        if (SwitchParametres.CanDash)
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
        _charSounds.PlayDashSound();
        yield return new WaitForSeconds(dashingTime);
        _rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion

    #region Walls sliding
    [Header("Wall slide")]

    [SerializeField] private float speedWallSliding;
    private bool isWallSliding = false;
    [SerializeField] private float distanceToWall;
    [SerializeField] private Transform wallsCheck;
    [SerializeField] private LayerMask wallLayer;

    private void WallSliding()
    {
        // Скольжение по стенам
        if (isWallSliding && !isGrounded && _rb.velocity.y < 0)
        {
            _rb.velocity = new Vector2(0, speedWallSliding);
            isWallSliding = true;
            isFalling = false;
            isGrounded = false;
        }
    }
    #endregion

    #region Attack
    [Header("Attack")]

    [SerializeField] private Transform attackCheck;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] public bool isCanAttack;
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

        if (Input.GetButtonDown("Attack") && SwitchParametres.CanAttack)
        {
            Debug.Log("Персонаж атакует");

            if (attackColliders.Length > 0 && canAttack)
            {
                target = attackColliders[0].GetComponent<IDamageToEnemy>();
                target.EnemyGetDamaged(this, damage);
                Debug.Log("Персонаж нанес урон");
                target = null;
            }
            else
            {
                if (target != null)
                {
                    Debug.Log(target);
                    target = null;
                    Debug.Log("Персонаж не нанес урон, рядом никого нет");
                }
            }

            StartCoroutine(AttackCoroutine());

            _animator.SetBool("IsAttacking", true);

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
        CanMove = false;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        _animator.SetBool("IsAttacking", false);
        CanMove = true;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    #endregion

    private void CanMoveFalse()
    {
        CanMove = false;
    }

    private void CanMoveTrue()
    {
        CanMove = true;
    }
}
