using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using System.Diagnostics;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        

        Debug.Log("Game Paused, pauseMenu active: " + pauseMenu.activeSelf);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
         // Reset the pause state

         // Resume the game
        Debug.Log("Game Resumed"); // Log for debugging purposes
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure time scale is reset before loading the main menu
        SceneManager.LoadScene("MainMenuScene");
        isPaused = false; // Reset the pause state
    }
}
