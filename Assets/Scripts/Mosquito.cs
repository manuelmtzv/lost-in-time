using System;
using Assets.interfaces;
using UnityEngine.UI;
using UnityEngine;
using System.Data.Common;

public class Mosquito : MonoBehaviour, IEnemy
{
  public PlayerState playerState;
  public EnemyState state;
  public float speed;
  public int health;
  public int damage;
  public GameObject bloodParticles;
  public GameObject smallBloodParticles;
  public SliderBar healthBar;
  public float baseSpeed;
  public float maxDistanceFromPlayer;
  public float maxDistanceFromPlayerAttack;
  public GameObject mosquitoSpit;
  private float attackDelayTimer;
  private Rigidbody2D rb;
  private Player player;
  private GameObject playerObject;
  private bool isTouchingAPlatform;


  void Start()
  {
    playerObject = GameObject.FindGameObjectWithTag("Player");
    player = playerObject.GetComponent<Player>();
    rb = GetComponent<Rigidbody2D>();

    speed = baseSpeed = UnityEngine.Random.Range(state.minSpeed, state.maxSpeed);
    health = state.health;
    damage = state.damage;

    healthBar.SetSliderMax(health);
  }

  void Update()
  {
    if (playerState.health <= 0) return;

    Vector3 difference = playerObject.transform.position - transform.position;

    if (maxDistanceFromPlayer <= difference.magnitude || isTouchingAPlatform)
    {
      transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
    }

    if (maxDistanceFromPlayerAttack >= difference.magnitude)
    {
      if (attackDelayTimer <= 0)
      {
        Attack();
        attackDelayTimer = CalculateNextAttackDelay();
      }
      else
      {
        attackDelayTimer -= Time.deltaTime;
      }
    }

    bool playerDirection = CheckUserPositionSide();
    Flip(playerDirection);
  }

  float CalculateNextAttackDelay()
  {
    return UnityEngine.Random.Range(2, 3);
  }

  void Attack()
  {
    GameObject spit = Instantiate(mosquitoSpit, transform.position, Quaternion.identity);
  }

  public void TakeDamage(int damage)
  {

    health -= damage;
    healthBar.SetSliderValue(health);

    if (health <= 0)
    {
      Kill();
    }
  }

  public void Kill()
  {
    AudioManager.Instance.PlaySFX(AudioManager.Instance.GetRandomClip(new[] { GlobalAssets.Instance.enemyDeathSoundOne, GlobalAssets.Instance.enemyDeathSoundTwo }), 0.2f);
    Instantiate(bloodParticles, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  bool CheckUserPositionSide()
  {
    return playerObject.transform.position.x > transform.position.x;
  }

  void Flip(bool direction = false)
  {
    bool mosquitoDirection = !(transform.localScale.x > 0);

    if (direction != mosquitoDirection)
    {
      transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
      healthBar.SetSliderDirection(mosquitoDirection ? Slider.Direction.LeftToRight : Slider.Direction.RightToLeft);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    isTouchingAPlatform = collision.CompareTag("Platform");
  }
}