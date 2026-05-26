using UnityEngine;

public class Honey : MonoBehaviour
{
    public GameObject victoryScreen;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        victoryScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            audioSource.Play();
            victoryScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void GoToTitleScreen()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
}
