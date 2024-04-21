using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandage : MonoBehaviour
{
    public int healAmount = 30;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerBody"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player.playerState.health < player.playerState.maxHealth)
            {
                player.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
