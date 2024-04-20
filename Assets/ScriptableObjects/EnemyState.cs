using UnityEngine;

[CreateAssetMenu(menuName = "Lost in time/EnemyState")]
public class EnemyState : ScriptableObject
{
    public float speed = 2f;
    public int health = 100;
    public int damage = 10;
}