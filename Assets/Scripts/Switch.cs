using UnityEngine;
using UnityEngine.InputSystem;

public class Switch : MonoBehaviour
{
    InputAction SwitchAction;
    public SpriteRenderer spriteRenderer;
    public Sprite offSprite;
    public Sprite onSprite;
    public GameObject ladder;
    bool isOn = false;
    bool canActivate = false;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = offSprite;
        SwitchAction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchAction.triggered && canActivate && !isOn)
        {
            audioSource.Play();
            spriteRenderer.sprite = onSprite;
            ladder.gameObject.transform.position = new Vector3(ladder.gameObject.transform.position.x, -1.5f, 0);
            isOn = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            canActivate = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            canActivate = false;
        }
    }
}
