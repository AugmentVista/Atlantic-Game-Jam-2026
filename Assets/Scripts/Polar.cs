using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Polar : MonoBehaviour
{
    
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite swimSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (canSwim(collider)) {
            GetComponentInParent<PlayerController>().CurrentState = PlayerController.State.InWater;
            spriteRenderer.sprite = swimSprite;
            GetComponentInParent<Rigidbody2D>().linearVelocityY = 0f;
            GetComponentInParent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (canSwim(collider))
        {
            GetComponentInParent<PlayerController>().CurrentState = PlayerController.State.InAir;
            spriteRenderer.sprite = defaultSprite;
            GetComponentInParent<Rigidbody2D>().gravityScale = GetComponentInParent<PlayerController>().gravityForce;
        }
    }

    bool canSwim(Collider2D collider)
    {
        return collider.gameObject.tag == "Water" && GetComponentInParent<PlayerController>().CurrentBear == PlayerController.Bear.Polar;
    }
}
