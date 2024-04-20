using UnityEngine;

[CreateAssetMenu(menuName = "Lost in time/PlayerState")]
public class PlayerState : ScriptableObject
{
    public int health = 200;
    public int maxHealth = 200;
    public bool IsDead => health <= 0;
}