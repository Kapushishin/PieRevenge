using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public ControllerCharacter _controller;
    private Animator _animator;

    private float runHorizontal;
    [SerializeField] private float runSpeed;

    private bool jump = false;
    private bool crouch = false;
    private bool dash = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        runHorizontal = Input.GetAxisRaw("Horizontal") * runSpeed;
        _animator.SetFloat("Speed", Mathf.Abs(runHorizontal));

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = true;
        }
    }

    private void FixedUpdate()
    {
        _controller.Move(runHorizontal * Time.fixedDeltaTime, crouch, jump, dash);
        jump = false;
        dash = false;
    }
}
