using System.Collections;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Collider2D topBodyCollider;
    private TrailRenderer _tr;
    private Animator _animator;

    private float runHorizontal;
    [SerializeField] private float speed;

    private float coyoteTime = 0.2f; //время, которое дается на то, что бы нажать прыжок сойдя с платформы
    private float coyoteTimeCounter;

    [SerializeField] private float jumpForce;
    private float jumpBufferTime = 0.2f; //буфер прыжка, который позволяет прыгнуть, когда персонаж еще не приземлился
    private float jumpBufferCounter;
    [SerializeField] private float jumpCooldown = 0.4f;

    private bool isFacingRight;

    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private bool isJumping;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isGrounded;
    private bool isFalling;
    

    private int playerLayerMask, platformLayerMask;
    [SerializeField] private float ignorelLayerTime;

    

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
        Flip();
    }

    private void FixedUpdate()
    {
        //IsGrounded();
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                isFalling = false;
            }
        }

        // если скорость меньше нуля, то включается анимация падения
        if (_rb.velocity.y < 0)
        {
            isFalling = true;
            _animator.SetBool("IsFalling", true);
            _animator.SetBool("IsJumping", false);
        }
        else
        {
            _animator.SetBool("IsFalling", false);
        }
    }

    private void Run(float speed)
    {
        if (isDashing) // когда персонаж находится в дэше, механика бега не будет его перебивать
            return;
        runHorizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(runHorizontal * speed, _rb.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(runHorizontal));
    }

    private void Crouching() // присед
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (isGrounded)
            {
                _animator.SetBool("IsCrouching", true);
                //_animator.Play("Player1_Crouch");
                isCrouching = true;
            }
        }

        if (isFalling)
            return;
        else if (Input.GetButtonUp("Crouch"))
        {
            _animator.SetBool("IsCrouching", false);
            _animator.Play("Player1_Get_up");
            isCrouching = false;
        }
    }

    private void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, false);
        isCrouching = false;
    }

    private void Jump()
    {
        if (isGrounded && !isCrouching  ) 
        {
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
            canDoubleJump = true;
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
            jumpBufferCounter = 0f; // для прыжков в воздухе
            canDoubleJump = false;
            isJumping = true;
            isGrounded = false;
            _animator.SetBool("IsJumping", true);
            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f && !isCrouching) // высокий прыжок
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
        if (isCrouching && Input.GetButtonDown("Jump") && isGrounded)
        {
            Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, true);
            _animator.SetBool("IsJumping", isJumping);
            Invoke("IgnoreLayerOff", ignorelLayerTime);
        }
    }

    public void Flip() //чтобы персонаж поворачивался по направлению движения
    {
        if (isFacingRight && runHorizontal > 0f || !isFacingRight && runHorizontal < 0f)
        {
            Vector3 _localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
        }
    }

    private void Dashing()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
        if (isDashing)
        {
            _rb.velocity = new Vector2(_rb.transform.localScale.x * dashingForce, 0f); //берем направление персонажа по х
        }
    }

    private IEnumerator JumpCooldown() //если очень быстро жать пробел, то делает двойной прыжок, так не надо
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f; // отключаем гравитацию, чтобы персонаж не двигался наискосок
        
        _tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
