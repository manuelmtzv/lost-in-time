
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float verticalOffset = 1f;

    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + verticalOffset, transform.position.z);
    }
}
