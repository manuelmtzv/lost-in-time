using UnityEngine;

public class MosquitoSpit : MonoBehaviour
{
  public Transform target;
  public float speed = 8f;
  public BloodParticles smallBloodParticles;
  private GameObject playerObject;
  private Player player;
  private Vector3 direction;
  private Quaternion rotation;

  void Start()
  {
    playerObject = GameObject.FindGameObjectWithTag("Player");
    player = playerObject.GetComponent<Player>();
    target = playerObject.transform;

    direction = target.position - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    transform.rotation = rotation;
  }

  void Update()
  {
    transform.position += speed * Time.deltaTime * transform.right;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      player.TakeDamage(25);
      Instantiate(smallBloodParticles, playerObject.transform.position, Quaternion.identity);
    }

    if (!other.CompareTag("Platform") || !other.CompareTag("Enemy"))
    {
      AudioManager.Instance.PlaySFX(GlobalAssets.Instance.missedProyectileSound, 0.28f);
      Destroy(gameObject);
    }
  }
}