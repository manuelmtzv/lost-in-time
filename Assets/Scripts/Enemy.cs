using System;
using System.Collections;
using System.Collections.Generic;
using Assets.interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public PlayerState playerState;
    public EnemyState state;
    public float speed;
    public int health;
    public int damage;
    public float attackDelay = 0.155f;
    public GameObject bloodParticles;
    public GameObject smallBloodParticles;
    public SliderBar healthBar;
    public float baseSpeed;
    public GameObject bandage;
    private Rigidbody2D rb;
    private float attackDelayTimer;
    private bool canMove = true;
    private bool isStunned = false;
    private float stunDuration = 0.3f;
    private Player player;
    private GameObject playerObject;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        attackDelayTimer = attackDelay;

        speed = baseSpeed = UnityEngine.Random.Range(state.minSpeed, state.maxSpeed);
        health = state.health;
        damage = state.damage;

        healthBar.SetSliderMax(health);
    }


    void Update()
    {
        if (playerState.health <= 0) return;

        Vector3 difference = playerObject.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90f);

        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
        }
        else
        {
            attackDelayTimer -= Time.deltaTime;

            if (attackDelayTimer <= 0)
            {
                player.TakeDamage(10);
                Instantiate(smallBloodParticles, playerObject.transform.position, Quaternion.identity);
                attackDelayTimer = attackDelay;
            }
        }
    }

    void FixedUpdate()
    {
        if (isStunned)
        {
            stunDuration -= Time.deltaTime;
            speed *= 0.75f;

            if (stunDuration <= 0)
            {
                isStunned = false;
                speed = baseSpeed;
            }
        }
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
        int bandageDropChance = UnityEngine.Random.Range(0, 100);
        player.playerState.enemiesKilled++;

        if (bandageDropChance <= 1.25f)
        {
            Instantiate(bandage, transform.position, Quaternion.identity);
        }

        Instantiate(bloodParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            stunDuration = duration;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) canMove = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) canMove = true;
    }
}
