using UnityEngine;

public class FixedSliderBar : MonoBehaviour
{
    public Transform target;
    public Camera gameCamera;

    void Start()
    {
        gameCamera = Camera.main;
        this.FixPosition();
    }

    void Update()
    {
        this.FixPosition();
    }

    void FixPosition()
    {
        transform.SetPositionAndRotation(target.position + new Vector3(0, 0.5f, 0), gameCamera.transform.rotation);
    }
}