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
    [SerializeField] private float jumpCurrent;
    [SerializeField] private float stockJumps = 1f;
    private bool isJumping;
    //[SerializeField] private float jumpCooldown = 0.4f;

    private bool isFacingRight;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private bool crouch;

    private int playerLayerMask, platformLayerMask;

    private bool isGrounded;

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
        IsGrounded();
        Crouching();
        Jump();

        _animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashCoroutine());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        if (isDashing) // когда персонаж находится в дэше, механика бега не будет его перебивать
        {
            return;
        }

        if (!crouch)
        {
            Run(speed);
        }
        else
        {
            Run(0f);
        }
    }

    private void IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                isJumping = false;
            }
        }
    }

    private void Run(float speed)
    {
        runHorizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(runHorizontal * speed, _rb.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(runHorizontal));
    }

    private void Crouching() // присед
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                topBodyCollider.enabled = false;
                _animator.SetBool("IsCrouching", true);
                _animator.Play("Player1_Crouch");
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                topBodyCollider.enabled = true;
                _animator.SetBool("IsCrouching", false);
                _animator.Play("Player1_Get_up");
                crouch = false;
            }
        }
    }

    private void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, false);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && crouch)
        {
            Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, true);
            _animator.SetBool("IsGrounded", isGrounded);
            Invoke("IgnoreLayerOff", 0.2f);
        }

        if (isGrounded) 
        {
            coyoteTimeCounter = coyoteTime;
            jumpCurrent = stockJumps;
            isJumping = false;
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

        if ((coyoteTimeCounter > 0f && jumpBufferCounter > 0f || Input.GetButtonDown("Jump") && jumpCurrent > 0 && !isGrounded) && !crouch) // прыжок
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f; // для прыжков в воздухе
            jumpCurrent--;
            isJumping = true;
            isGrounded = false;
            //StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f) // высокий прыжок
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
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

    /*private IEnumerator JumpCooldown() //если очень быстро жать пробел, то делает двойной прыжок, так не надо
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }*/

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f; // отключаем гравитацию, чтобы персонаж не двигался наискосок
        _rb.velocity = new Vector2(_rb.transform.localScale.x * dashingForce, 0f); //берем направление персонажа по х
        _tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
