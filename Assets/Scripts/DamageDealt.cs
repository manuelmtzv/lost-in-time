using TMPro;
using UnityEngine;

public class DamageDealt : MonoBehaviour
{
    public PlayerState playerState;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = playerState.damageDealt.ToString();
    }
}