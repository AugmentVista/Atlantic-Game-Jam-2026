using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsScreen;
    public GameObject howToPlayScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu.SetActive(true);
        creditsScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        creditsScreen.SetActive(true);
        howToPlayScreen.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        mainMenu.SetActive(false);
        creditsScreen.SetActive(false);
        howToPlayScreen.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        creditsScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
    }
}
