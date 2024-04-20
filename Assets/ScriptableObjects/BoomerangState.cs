using System.Runtime.Serialization;
using UnityEngine;

public enum BoomerangLifeCycle
{
    Idle,
    Thrown,
    Returning
}

[CreateAssetMenu(menuName = "Lost in time/BoomerangState")]
public class BoomerangState : ScriptableObject
{
    public float throwForce = 5f;
    public float returnForce = 6f;
    public float rotateForce = 500f;
    public float throwTime = 1.2f;
    public float baseDamage = 10f;
    public float critMultiplier = 1.75f;
    public BoomerangLifeCycle state = BoomerangLifeCycle.Idle;


}
