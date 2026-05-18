using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Panda : MonoBehaviour
{
    InputAction MoveAction;
    
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite climbSprite;
    
    Vector2 move;

    bool canClimb = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool CanClimb { get => canClimb; set => canClimb = value; }
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        handleClimb();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Climbable")
        {
            canClimb = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Climbable")
        {
            spriteRenderer.sprite = defaultSprite;
            GetComponentInParent<PlayerController>().CurrentState = PlayerController.State.InAir;
            GetComponentInParent<Rigidbody2D>().gravityScale = GetComponentInParent<PlayerController>().gravityForce;
            canClimb = false;
        }
    }

    void handleClimb()
    {
        if (canClimb)
        {
            if (move.y != 0)
            {
                GetComponentInParent<PlayerController>().CurrentState = PlayerController.State.Climbing;
                spriteRenderer.sprite = climbSprite;
            }
        }
    }
}
