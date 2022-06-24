using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Collider2D topBodyCollider;
    private TrailRenderer _tr;
    private Animator _animator;

    private Vector3 curVelocity = Vector3.zero;
    [SerializeField] private float smoothMovement;

    private float coyoteTime = 0.2f; //время, которое дается на то, что бы нажать прыжок сойдя с платформы
    private float coyoteTimeCounter;

    [SerializeField] private float jumpForce;
    private float jumpBufferTime = 0.2f; //буфер прыжка, который позволяет прыгнуть, когда персонаж еще не приземлился
    private float jumpBufferCounter;
    private bool canDoubleJump = true;
    private bool isJumping;
    //[SerializeField] private float jumpCooldown = 0.4f;

    private bool isFacingRight;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private bool isCrouching;

    private int playerLayerMask, platformLayerMask;

    [SerializeField] private bool isGrounded;
    private bool canAirControl = true;
    private float jumpTime = 0;
    private float jumpHeight = 14f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
        _animator = GetComponent<Animator>();

        playerLayerMask = LayerMask.NameToLayer("Player");
        platformLayerMask = LayerMask.NameToLayer("Platform");
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        // Проверяем на земле ли персонаж
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                isJumping = false;
                //canDoubleJump = true;
                jumpTime = 0f;
            }
        }
        // если скорость меньше нуля, то персонаж падает
        if (_rb.velocity.y < 0)
        {
            _animator.SetBool("IsFalling", true);
        }
        else
        {
            _animator.SetBool("IsFalling", false);
        }
    }

    public void Move(float move, bool crouch, bool jump, bool dash)
    {
        // рывок
        if (dash && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
        if (isDashing)
        {
            _rb.velocity = new Vector2(_rb.transform.localScale.x * dashingForce, 0f);
        }

        else if (isGrounded || canAirControl)
        {
            // передвижение персонажа
            Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref curVelocity, smoothMovement);
            // поворот по направлению движения
            if (move > 0f && isFacingRight)
            {
                Flip();
            }
            else if (move < 0f && !isFacingRight)
            {
                Flip();
            }
            // приседание
            if (crouch)
            {
                if (!isCrouching)
                {
                    _animator.SetBool("IsCrouching", true);
                    _animator.Play("Player1_Crouch");
                    isCrouching = true;
                }
            }
            else if (isCrouching)
            {
                _animator.SetBool("IsCrouching", false);
                _animator.Play("Player1_Get_up");
                isCrouching = false;
            }
            // спрыгивание с платформы
            if (jump && crouch)
            {
                Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, true);
                Invoke("IgnoreLayerOff", 0.2f);
            }
        }
            // прыжок
        if (isGrounded && jump && !crouch)
        {
            if (jumpTime++ < jumpHeight)
            {
                //_rb.AddForce(Vector2.up * jumpForce);
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                isJumping = true;
                isGrounded = false;
                canDoubleJump = true;
                Debug.Log("jumped");
                Debug.Log(jumpTime.ToString());
            }
            //_rb.AddForce(Vector2.up * jumpForce);
            /*jumpTime += Time.fixedDeltaTime;
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            isJumping = true;
            isGrounded = false;
            canDoubleJump = true;
            Debug.Log("jumped");
            Debug.Log(jumpTime.ToString());*/
        }
        

        // двойной прыжок
        else if (!isGrounded && canDoubleJump && jump)
        {
            canDoubleJump = false;
            Debug.Log("double jumped");
            //_rb.velocity = new Vector2(_rb.velocity.x, 0);
            //_rb.AddForce(new Vector2(0f, jumpForce / 1.2f));
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }

        



        /*if (isGrounded) 
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
        }*/
    }

    // выключение игнорирования коллайдеров персонажа и платформ
    private void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, platformLayerMask, false);
    }

    private void Flip() //чтобы персонаж поворачивался по направлению движения
    {
        Vector3 _localScale = transform.localScale;
        isFacingRight = !isFacingRight;
        _localScale.x *= -1f;
        transform.localScale = _localScale;
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
