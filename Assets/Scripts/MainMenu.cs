using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //Main Menu

    const int numOfLevels = 5;
    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().buildIndex <= (numOfLevels-1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            //go back to main menu if no screens left
            SceneManager.LoadScene(0);
        }
        
    }

    //quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
