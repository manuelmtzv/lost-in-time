using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameStateManager : MonoBehaviour
{
    public PlayerState playerState;
    public GameState gameState;

    void Start()
    {
        gameState.isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseGame();
        }
    }

    public void TogglePauseGame()
    {
        gameState.isPaused = !gameState.isPaused;
        Time.timeScale = gameState.isPaused ? 0 : 1;
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