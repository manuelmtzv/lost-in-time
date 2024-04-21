using TMPro;
using UnityEngine;

public class EnemiesKilled : MonoBehaviour
{
    public PlayerState playerState;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "Enemies killed: " + playerState.enemiesKilled.ToString();
    }
}