using UnityEngine;

[CreateAssetMenu(menuName = "Lost in time/PlayerState")]
public class PlayerState : ScriptableObject
{
    public int damageDealt = 0;
    public int enemiesKilled = 0;
    public int health = 200;
    public int maxHealth = 200;
    public bool IsDead => health <= 0;

    public void Reset()
    {
        damageDealt = 0;
        enemiesKilled = 0;
        health = maxHealth;
    }
}