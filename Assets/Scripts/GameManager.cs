using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int castleHealth = 100;
    public int currentLevel = 1;
    public int maxLevels = 3;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
            RestartLevel();
        }
    }

    public void LevelCompleted()
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

    private void RestartLevel()
    {
        Debug.Log("Castle destroyed! Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void WinGame()
    {
        Debug.Log("You won the game!");
        SceneManager.LoadScene("MainMenuScene");
    }
}
