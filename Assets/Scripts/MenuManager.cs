using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public PlayerState playerState;
    public GameState gameState;
    public bool showPausedMenu;
    public GameObject globalMenuOverlay;
    public GameObject pauseMenu;

    void Update()
    {
        showPausedMenu = gameState.isPaused;

        globalMenuOverlay.SetActive(showPausedMenu);
        pauseMenu.SetActive(showPausedMenu);
    }
}