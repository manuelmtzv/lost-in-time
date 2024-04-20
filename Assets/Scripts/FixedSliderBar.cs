using UnityEngine;

public class FixedSliderBar : MonoBehaviour
{
    public Camera gameCamera;
    public Transform target;

    void Update()
    {
        transform.rotation = gameCamera.transform.rotation;
        transform.position = target.position + new Vector3(0, 0.5f, 0);
    }
}