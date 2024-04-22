using UnityEngine;

[CreateAssetMenu(menuName = "Lost in time/GameState")]
public class GameState : ScriptableObject
{
    public bool isPaused;
    public bool isGameOver;

    public void ResetState()
    {
        isPaused = false;
        isGameOver = false;
    }
}