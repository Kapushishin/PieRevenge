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

    [SerializeField] private bool isGrounded;
    private bool isFalling;

    private int playerLayerMask, platformLayerMask;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
        _animator = GetComponent<Animator>();

        playerLayerMask = LayerMask.NameToLayer("Player");
        platformLayerMask = LayerMask.NameToLayer("Platform");
    }

    private void Update()
    {
        if (isCrouching) // чтобы не бегал во время приседа
            Run(0f);
        else
            Run(speed);

        
        Dashing();
        Jump();
        Crouching();
        WallSliding();
        //Attacking();
        //Landing();

        _animator.SetBool("IsJumping", isJumping);
        _animator.SetBool("IsFalling", isFalling);
        _animator.SetBool("IsWallSliding", isWallSliding);
        _animator.SetBool("IsDashing", isDashing);
    }

    private void FixedUpdate()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
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

        if (isFacingRight && runHorizontal > 0f || !isFacingRight && runHorizontal < 0f)
        {
            Vector3 _localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
        }
    }
    #endregion

    #region Crouching

    [Header("Crouch")]

    private bool isCrouching;
    private void Crouching() 
    {
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
    //время, которое дается на то, что бы нажать прыжок сойдя с платформы
    private float coyoteTime = 0.2f; 
    private float coyoteTimeCounter;
    //буфер прыжка, который позволяет прыгнуть, когда персонаж еще не совсем приземлился
    private float jumpBufferTime = 0.2f; 
    private float jumpBufferCounter;
    private float jumpCooldown = 0.4f;
    private bool isJumping;
    private bool isJumpingOff;
    [SerializeField] private bool canDoubleJump;

    private float ignoreLayerTime = 0.3f;
    private float cantCrouchingJump = 0.5f;

    private void Jump()
    {
        if (isGrounded && !isCrouching) 
        {
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
            canDoubleJump = true;
            isJumpingOff = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((coyoteTimeCounter > 0f && jumpBufferCounter > 0f || Input.GetButtonDown("Jump") && canDoubleJump && !isGrounded) && !isCrouching) // прыжок
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            // для прыжков в воздухе
            jumpBufferCounter = 0f; 
            canDoubleJump = false;
            isJumping = true;
            //StartCoroutine(JumpCooldown());
        }

        // высокий прыжок
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f && !isCrouching) 
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (isCrouching && Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumpingOff = true;
            Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, true);
            Invoke("IgnoreLayerOff", ignoreLayerTime);
            Invoke("CantCrouchingJump", cantCrouchingJump);
        }
    }
    //если очень быстро жать пробел, то делает двойной прыжок, так не надо
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
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(DashCoroutine());
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

    private bool canAttack = true;
    private bool isAttacking = false;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackTime;
    private int attackAnimCondition = 0;

    private void Attacking()
    {
        if (Input.GetButtonDown("Attack") && canAttack)
        {
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
}
