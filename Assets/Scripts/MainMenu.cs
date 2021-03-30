using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //Main Menu

    const int numOfLevels = 5;
    public static bool AimAssistON = true;

    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().buildIndex <= (numOfLevels-1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // This should only be called upon entering the actual level
            // If this method is also used to go to the game over scene, 
            // This call will have to be moved 
            QualtricsDataContainer.Start();
        }
        else
        {
            //go back to main menu if no screens left
            SceneManager.LoadScene(0);
        }
    }

    //Options Menu
    public void ToggleAimAssist()
    {
        AimAssistON = !AimAssistON;

        QualtricsDataContainer.SetAccessibilityFeature("AimAssist", AimAssistON); 
    }

    //quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
