
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum Bears{
        Polar,
        Grizz,
        Panda
    };

    enum States{
        OnGround,
        InAir,
        InWater,
        CroucingOnGround,
        Climbing,
    };
    
    Rigidbody2D rb;
    InputAction MoveAction;
    InputAction JumpAction;
    InputAction NextBearAction;
    InputAction PreviousBearAction;
    InputAction BearAction;

    public float speed = 3.0f;
    public float jumpForce = 10.0f;
    public float swimForce = 5.0f;

    public Bears startingBear = Bears.Polar;

    public SpriteRenderer polarSpriteRenderer;
    public SpriteRenderer grizzSpriteRenderer;
    public SpriteRenderer pandaSpriteRenderer;

    public Sprite polarDefaultSprite;
    public Sprite polarInWaterSprite;
    public Sprite grizzDefaultSprite;
    public Sprite grizzCrouchSprite;
    public Sprite pandaDefaultSprite;
    public Sprite pandaClimbSprite;


    States currentState = States.OnGround;

    Bears currentBear;

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
        currentBear = startingBear;
        swapBears();
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
        /*if (currentBear == Bears.Polar && currentState == States.InWater)
        {
            Debug.Log("Swimming in water with polar bear!");
            rb.AddForce(Vector2.up * swimForce, ForceMode2D.Force); // Reduced vertical control in water
        }*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log("Collision entered with layer: " + LayerMask.LayerToName(collision.gameObject.layer) + " at point: " + contact.point);
        Debug.Log(contact.normal);
        switch(LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Ground":
                switch(currentState)
                {
                    case States.InAir:
                        currentState = States.OnGround;
                        break;
                    case States.InWater:
                    Debug.Log("Exiting water and landing on ground with polar bear!");
                        rb.AddForceY(4f, ForceMode2D.Impulse); // Small upward force to prevent sticking to the ground
                        break;
                }
                break;
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger entered with layer: " + LayerMask.LayerToName(collider.gameObject.layer));
        if (currentBear == Bears.Polar && collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            currentState = States.InWater;
            polarSpriteRenderer.sprite = polarInWaterSprite;
            //rb.linearVelocityY = 0f;
            rb.gravityScale = 1f;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Trigger exited with layer: " + LayerMask.LayerToName(collider.gameObject.layer));
        if (currentBear == Bears.Polar && collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            currentState = States.OnGround;
            polarSpriteRenderer.sprite = polarDefaultSprite;
            rb.gravityScale = 3f;
        }
    }

    void handleMove()
    {
        move = MoveAction.ReadValue<Vector2>();
    }
    
    void handleJump()
    {
        if (JumpAction.triggered && currentState == States.OnGround)
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            currentState = States.InAir;
        }
    }

    void handleBearSwitch()
    {
        if (NextBearAction.triggered)
        {
            currentBear = (Bears)(((int)currentBear + 1) % Enum.GetNames(typeof(Bears)).Length);
            swapBears();
        }
        if (PreviousBearAction.triggered)
        {
            currentBear = (Bears)(((int)currentBear - 1 + Enum.GetNames(typeof(Bears)).Length) % Enum.GetNames(typeof(Bears)).Length);
            swapBears(); 
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
                    if (currentState == States.OnGround)
                    {
                        currentState = States.CroucingOnGround;
                        grizzSpriteRenderer.sprite = grizzCrouchSprite;
                    }
                    else if (currentState == States.CroucingOnGround)
                    {
                        currentState = States.OnGround;
                        grizzSpriteRenderer.sprite = grizzDefaultSprite;
                    }
                    break;
                case Bears.Panda:
                    // Panda bear action
                    Debug.Log("Panda bear action executed!");
                    break;
            }
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
}