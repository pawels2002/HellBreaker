using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameOverScript : MonoBehaviour
{
    public GameObject gameOverScreen;

    void Start()
    {
        gameOverScreen.SetActive(false);
    }

    public void LoseGame()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
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
}
