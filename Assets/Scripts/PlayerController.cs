
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum Bear{
        Polar,
        Grizz,
        Panda
    };

    public enum State{
        OnGround,
        InAir,
        InWater,
        Climbing,
    };
    
    Rigidbody2D rb;
    InputAction MoveAction;
    InputAction JumpAction;
    InputAction NextBearAction;
    InputAction PreviousBearAction;
    public float speed = 3.0f;
    public float jumpForce = 10.0f;
    public float leaveWaterForce = 9.0f;
    public float gravityForce = 3.0f;
    public float crouchScale = 0.8f;

    public Bear startingBear = Bear.Polar;

    State currentState = State.OnGround;

    Bear currentBear;

    Vector2 move;

    public State CurrentState { get => currentState; set => currentState = value; }
    public Bear CurrentBear { get => currentBear; set => currentBear = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        NextBearAction = InputSystem.actions.FindAction("Next");
        PreviousBearAction = InputSystem.actions.FindAction("Previous");
        currentBear = startingBear;
        rb.gravityScale = gravityForce;
        swapBears();
    }

    // Update is called once per frame
    void Update()
    {
        handleMove();
        handleJump();
        handleBearSwitch();
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = move.x * speed;
        if (canClimb() && currentState == State.Climbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocityY = move.y * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                switch(currentState)
                {
                    case State.InAir:
                        currentState = State.OnGround;
                        break;
                    case State.InWater:
                        Debug.Log("Exiting water and landing on ground with polar bear!");
                        rb.AddForceY(leaveWaterForce, ForceMode2D.Impulse); // Small bounce when exiting water
                        break;
                }
                break;
        }

    }

    void handleMove()
    {
        move = MoveAction.ReadValue<Vector2>();
    }
    
    void handleJump()
    {
        if (JumpAction.triggered && 
            (currentState == State.OnGround || 
            (currentBear == Bear.Polar && currentState == State.InWater)
        ))
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            currentState = State.InAir;
        }
    }

    void handleBearSwitch()
    {
        if (currentState == State.InWater) return; 
        if (NextBearAction.triggered)
        {
            currentBear = (Bear)(((int)currentBear + 1) % Enum.GetNames(typeof(Bear)).Length);
            swapBears();
        }
        if (PreviousBearAction.triggered)
        {
            currentBear = (Bear)(((int)currentBear - 1 + Enum.GetNames(typeof(Bear)).Length) % Enum.GetNames(typeof(Bear)).Length);
            swapBears(); 
        }
    }

    void swapBears()
    {
        int yOffset = 0;
        for (int i = (int)currentBear; i < (int)currentBear + 3; i++)
        {
            Transform bear = transform.GetChild(i % 3);
            bear.localPosition = new Vector3(0, yOffset++);
        }
    }

    Boolean canClimb()
    {
        return currentBear == Bear.Panda && transform.GetChild((int)Bear.Panda).GetComponent<Panda>().CanClimb;
    }
}