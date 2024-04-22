using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public PlayerState playerState;
    public GameState gameState;
    public GameObject globalMenuOverlay;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    private bool showPausedMenu;
    private bool showGameOverMenu;

    void Update()
    {
        showPausedMenu = gameState.isPaused && !gameState.isGameOver;
        showGameOverMenu = gameState.isGameOver;

        globalMenuOverlay.SetActive(showPausedMenu);

        // Menu's to show
        pauseMenu.SetActive(showPausedMenu);
        gameOverMenu.SetActive(showGameOverMenu);
    }
}