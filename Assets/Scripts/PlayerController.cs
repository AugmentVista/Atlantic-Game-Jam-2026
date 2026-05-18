
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
    public float leaveWaterForce = 9.0f;
    public float gravityForce = 3.0f;
    public float crouchScale = 0.8f;

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

    public GameObject pandaPrefab;
    public GameObject polarPrefab;
    public GameObject platformPrefab;
    GameObject pandaClone;
    GameObject polarClone;
    GameObject platform;

    States currentState = States.OnGround;
    Boolean canClimb = false;
    Boolean isCrouching = false;

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
        rb.gravityScale = gravityForce;
        swapBears();
    }

    // Update is called once per frame
    void Update()
    {
        handleMove();
        handleJump();
        handleClimb();
        handleBearSwitch();
        handleBearAction();
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = move.x * speed;
        if (currentBear == Bears.Panda && currentState == States.Climbing)
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
                    case States.InAir:
                        currentState = States.OnGround;
                        break;
                    case States.InWater:
                        Debug.Log("Exiting water and landing on ground with polar bear!");
                        rb.AddForceY(leaveWaterForce, ForceMode2D.Impulse); // Small bounce when exiting water
                        break;
                }
                break;
            case "Platform":
                isCrouching = false;
                grizzSpriteRenderer.sprite = grizzDefaultSprite;
                rb.transform.localScale = new Vector3(1, 1, 1);
                Destroy(polarClone);
                Destroy(pandaClone);
                Destroy(platform);
                transform.GetChild((int)Bears.Polar).gameObject.SetActive(true);
                transform.GetChild((int)Bears.Panda).gameObject.SetActive(true);
                transform.GetChild((int)Bears.Grizz).localPosition = Vector3.zero;
                transform.GetChild((int)Bears.Panda).localPosition = new Vector3(0, 1, 0); 
                transform.GetChild((int)Bears.Polar).localPosition = new Vector3(0, 2, 0); 
                break;
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Water":
                if (currentBear == Bears.Polar)
                {
                    currentState = States.InWater;
                polarSpriteRenderer.sprite = polarInWaterSprite;
                rb.linearVelocityY = 0f;
                rb.gravityScale = 0f;
                }
                break;
            case "Climbable":
                canClimb = true;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Water":
                if (currentBear == Bears.Polar)
                {
                    currentState = States.InAir;
                    polarSpriteRenderer.sprite = polarDefaultSprite;
                    rb.gravityScale = gravityForce;
                }
                break;
            case "Climbable":
                canClimb = false;
                if (currentState == States.Climbing)
                {
                    currentState = States.InAir;
                    rb.gravityScale = gravityForce;
                    pandaSpriteRenderer.sprite = pandaDefaultSprite;
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
            (currentState == States.OnGround || 
            (currentBear == Bears.Polar && currentState == States.InWater)
        ))
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            currentState = States.InAir;
        }
    }

    void handleClimb()
    {
        if (canClimb && currentBear == Bears.Panda)
        {
            if (move.y != 0)
            {
                currentState = States.Climbing;
                pandaSpriteRenderer.sprite = pandaClimbSprite;
            }
        }
    }

    void handleBearSwitch()
    {
        if (currentState == States.InWater) return; 
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
                    if (!isCrouching)
                    {
                        isCrouching = true;
                        rb.transform.localScale = new Vector3(1, crouchScale, 1); 
                        grizzSpriteRenderer.sprite = grizzCrouchSprite;
                        transform.GetChild((int)Bears.Polar).gameObject.SetActive(false);
                        transform.GetChild((int)Bears.Panda).gameObject.SetActive(false);
                        pandaClone = Instantiate(pandaPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity); 
                        polarClone = Instantiate(polarPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                        platform = Instantiate(platformPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
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