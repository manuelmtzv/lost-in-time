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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
        PauseGame();
    }

    public void RestartGame()
    {
        gameState.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}