using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] menuButtons;
    public RectTransform indicator;
    public GameObject howToPlayPanel;

    [Header("Settings")]
    public string playSceneName = "GameScene";

    private int selectedIndex = 0;
    private bool inHowToPlay = false;

    void Start()
    {
        UpdateIndicatorPosition();
        howToPlayPanel.SetActive(false);
    }

    void Update()
    {
        if (inHowToPlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseHowToPlay();
            }
            return;
        }

        // Navigation
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            UpdateIndicatorPosition();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateIndicatorPosition();
        }

        // Select
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    void UpdateIndicatorPosition()
    {
        if (indicator != null && selectedIndex >= 0 && selectedIndex < menuButtons.Length)
        {
            indicator.position = new Vector3(
                indicator.position.x,
                menuButtons[selectedIndex].transform.position.y,
                indicator.position.z
            );
        }
    }

    // Button Methods
    public void PlayGame()
    {
        Debug.Log("Play Game");
        SceneManager.LoadScene(playSceneName);
    }

    public void OpenHowToPlay()
    {
        Debug.Log("Open How To Play");
        howToPlayPanel.SetActive(true);
        inHowToPlay = true;
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        inHowToPlay = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
