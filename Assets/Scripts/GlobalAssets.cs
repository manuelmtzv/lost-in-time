using UnityEngine;

public class GlobalAssets : MonoBehaviour
{
    private static GlobalAssets _instance;

    public static GlobalAssets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalAssets>();
            }

            return _instance;
        }
    }

    public GameObject damagePopupPrefab;
}