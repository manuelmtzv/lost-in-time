using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameStateManager : MonoBehaviour
{
    public PlayerState playerState;
    public GameState gameState;

    void Awake()
    {
        gameState.ResetState();
        ResumeGame();
    }

    void Start()
    {
        AudioManager.Instance.PlayMusic(GlobalAssets.Instance.backgroundMusic);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }

        if (playerState.health <= 0)
        {
            EndGame();
        }
    }

    public void TogglePauseGame()
    {
        if (gameState.isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gameState.isPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gameState.isPaused = false;
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        gameState.isGameOver = true;
    }

    public void RestartGame()
    {
        gameState.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}