using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyState state;
    public float speed;
    public int health;
    public int damage;
    public float attackDelay = 0.4f;
    public GameObject bloodParticles;
    private float attackDelayTimer;
    private bool canMove = true;
    private Player player;
    private GameObject playerObject;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        attackDelayTimer = attackDelay;

        speed = state.speed;
        health = state.health;
        damage = state.damage;
    }


    void Update()
    {
        Vector3 difference = playerObject.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90f);
    }

    void FixedUpdate()
    {
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
                attackDelayTimer = attackDelay;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
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
