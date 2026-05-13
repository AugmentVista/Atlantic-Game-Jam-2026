
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum Bears{
        Polar,
        Grizz,
        Panda
    };
    
    Rigidbody2D rb;
    InputAction MoveAction;
    InputAction JumpAction;
    InputAction NextBearAction;
    InputAction PreviousBearAction;
    InputAction BearAction;

    public float speed = 3.0f;
    public float jumpForce = 10.0f;
    bool isGrounded = true;

    Bears currentBear = Bears.Polar;

    Vector2 move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        NextBearAction = InputSystem.actions.FindAction("Next");
        PreviousBearAction = InputSystem.actions.FindAction("Previous");
        BearAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        handleMove();
        handleJump();
        handleBearSwitch();
        handleBearAction();
    }

    void FixedUpdate()
    {
        //Vector2 position = (Vector2)rb.position + move * speed * Time.deltaTime;
        rb.linearVelocityX = move.x * speed;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void handleMove()
    {
        move = MoveAction.ReadValue<Vector2>();
    }
    void handleJump()
    {
        if (JumpAction.triggered && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void handleBearSwitch()
    {
        if (NextBearAction.triggered)
        {
            
            foreach (Transform bear in transform)
            {
                if (bear.name != currentBear.ToString())
                {
                    bear.localPosition += new Vector3(0, -1);
                }
                else
                {
                    bear.localPosition += new Vector3(0, 2);
                }
            }
            currentBear = (Bears)(((int)currentBear + 1) % Enum.GetNames(typeof(Bears)).Length);
        }
        if (PreviousBearAction.triggered)
        {
            currentBear = (Bears)(((int)currentBear - 1 + Enum.GetNames(typeof(Bears)).Length) % Enum.GetNames(typeof(Bears)).Length);
            foreach (Transform bear in transform)
            {
                if (bear.name != currentBear.ToString())
                {
                    bear.localPosition += new Vector3(0, 1);
                }
                else
                {
                    bear.localPosition += new Vector3(0, -2);
                }
            }            
        }
    }

    void handleBearAction()
    {
        if (BearAction.triggered)
        {   
            // Implement bear-specific actions here
            switch(currentBear)
            {
                case Bears.Polar:
                    // Polar bear action
                    Debug.Log("Polar bear action executed!");
                    break;
                case Bears.Grizz:
                    // Grizzly bear action
                    Debug.Log("Grizzly bear action executed!");
                    break;
                case Bears.Panda:
                    // Panda bear action
                    Debug.Log("Panda bear action executed!");
                    break;
            }
        }
    }
}