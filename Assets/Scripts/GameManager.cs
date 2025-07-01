using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverScreen;
    public GameObject winLevelScreen;
    public int castleHealth = 100;
    public int currentLevel = 1;
    public int maxLevels = 3;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameOverScreen.SetActive(false);
            winLevelScreen.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CastleTakeDamage(int damage)
    {
        castleHealth -= damage;
        Debug.Log($"Castle health: {castleHealth}");

        if (castleHealth <= 0)
        {
            LoseGame();
        }
    }
    public void CompleteLevel()
    {
        winLevelScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    public void NextLevel()
    {
        if (currentLevel < maxLevels)
        {
            currentLevel++;
            SceneManager.LoadScene($"Level{currentLevel}");
        }
        else
        {
            WinGame();
        }
    }

    public void LoseGame()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverScreen.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void WinGame()
    {
        Debug.Log("You won the game!");
        SceneManager.LoadScene("MainMenuScene");
    }
}
