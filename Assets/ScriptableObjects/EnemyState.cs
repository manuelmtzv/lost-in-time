using UnityEngine;

[CreateAssetMenu(menuName = "Lost in time/EnemyState")]
public class EnemyState : ScriptableObject
{
    public float minSpeed = 1.5f;
    public float maxSpeed = 2.8f;
    public int health = 100;
    public int damage = 10;
}