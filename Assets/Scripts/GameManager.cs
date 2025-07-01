using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    GameObject gameOverScreen;
    GameObject winLevelScreen;
    public int castleHealth = 100;
    public int currentLevel = 1;
    public int maxLevels = 3;
    private bool gameEnded = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AssignSceneUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignSceneUI();
        if (scene.name.StartsWith("Level"))
        {
            castleHealth = 100;
            gameEnded = false;
            Time.timeScale = 1f;
        }
    }

    private void AssignSceneUI()
    {
        gameOverScreen = GameObject.Find("GameOverPanel");
        winLevelScreen = GameObject.Find("WinLevelScreen");

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (winLevelScreen != null)
            winLevelScreen.SetActive(false);
    }


    public void CastleTakeDamage(int damage)
    {
        
        if (gameEnded || castleHealth <= 0)
            return;

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
        if (gameEnded)
            return;
        gameEnded = true;
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        gameEnded = false;
        castleHealth = 100;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
