using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{

    public void ChooseLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ChooseLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void ChooseLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Return()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
