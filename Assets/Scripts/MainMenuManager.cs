using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(GlobalAssets.Instance.mainMenuMusic);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
