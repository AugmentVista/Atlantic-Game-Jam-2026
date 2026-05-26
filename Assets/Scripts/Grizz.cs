using UnityEngine;
using UnityEngine.InputSystem;

public class Grizz : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite crouchSprite;

    public GameObject originalPanda;
    public GameObject originalPolar;
    public GameObject pandaPrefab;
    public GameObject polarPrefab;
    public GameObject platformPrefab;
    GameObject pandaClone;
    GameObject polarClone;
    GameObject platform;

    public float crouchScale = 0.8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GetComponentInParent<PlayerController>().CurrentBear == PlayerController.Bear.Grizz && 
            GetComponentInParent<PlayerController>().CurrentState != PlayerController.State.Crouching && 
            collider.gameObject.tag == "Cave")
        {
            GetComponentInParent<PlayerController>().CurrentState = PlayerController.State.Crouching;
            spriteRenderer.sprite = crouchSprite;
            GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3(1, crouchScale, 1);
            originalPanda.SetActive(false);
            originalPolar.SetActive(false);
            pandaClone = Instantiate(pandaPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity); 
            polarClone = Instantiate(polarPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            platform = Instantiate(platformPrefab, transform.position + new Vector3(0, .5f, 0), Quaternion.identity);

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (GetComponentInParent<PlayerController>().CurrentBear == PlayerController.Bear.Grizz && 
            GetComponentInParent<PlayerController>().CurrentState == PlayerController.State.Crouching && 
            collider.gameObject.tag == "Cave")
        {
            GetComponentInParent<PlayerController>().CurrentState = PlayerController.State.OnGround;
            spriteRenderer.sprite = defaultSprite;
            GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3(1, 1, 1);
            Destroy(polarClone);
            Destroy(pandaClone);
            Destroy(platform);
            originalPolar.gameObject.SetActive(true);
            originalPanda.gameObject.SetActive(true);
            transform.localPosition = Vector3.zero;
            originalPanda.transform.localPosition = new Vector3(0, 1, 0); 
            originalPolar.transform.localPosition = new Vector3(0, 2, 0);
        }
    }
}
