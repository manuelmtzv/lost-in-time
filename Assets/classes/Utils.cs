using UnityEngine;

public class Utils : MonoBehaviour
{
    public static bool GetRandomPosibility(float chance)
    {
        return Random.Range(0f, 1f) <= chance;
    }
}