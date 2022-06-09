using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private LayerMask groundLayer;
    private TrailRenderer _tr;

    private float runHorizontal;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private float coyoteTime = 0.2f; //время, которое дается на то, что бы нажать прыжок сойдя с платформы
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f; //буфер прыжка, который позволяет прыгнуть, когда персонаж еще не приземлился
    private float jumpBufferCounter;

    private float jumpCurrent;
    [SerializeField] private float stockJumps = 1f;
    private bool isJumping;
    [SerializeField] private float jumpCooldown = 0.4f;

    private bool isFacingRight;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (isDashing) // если персонаж находится в дэше, то ничего другого не прожать
        {
            return;
        }

        Jump();
        HighJump();
        MultyJump();
        Dash();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing) // если персонаж находится в дэше, то ничего другого не прожать
        {
            return;
        }

        Run();
    }

    public void Run()
    {
        runHorizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(runHorizontal * speed, _rb.velocity.y);
    }

    private bool IsGrounded() //для проверки находится ли персонаж на земле, создав в ногах круглый коллайдер
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Jump()
    {
        if (IsGrounded()) 
        {
            coyoteTimeCounter = coyoteTime;
            jumpCurrent = stockJumps;
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

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }
    }

    public void HighJump()
    {
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    public void MultyJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCurrent > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            jumpCurrent--;
        }
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Flip() //чтобы персонаж поворачивался по направлению движения
    {
        if (isFacingRight && runHorizontal < 0f || !isFacingRight && runHorizontal > 0f)
        {
            Vector3 _localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
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
        _rb.velocity = new Vector2(runHorizontal * dashingForce, 0f); //берем направление персонажа по х
        _tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
