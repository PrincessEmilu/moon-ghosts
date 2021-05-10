using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void OnTryAgain()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void OnKickAss()
    {
        SceneManager.LoadScene("EmMoonLevel");
    }

    public void OnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
