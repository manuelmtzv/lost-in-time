using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;



public class Boomerang : MonoBehaviour
{
    [SerializeField] GameObject boomerangArt;
    [SerializeField] BoomerangState boomerangState;
    public TimeFlowState timeFlowState;
    private Transform startPoint;
    private float timer;
    private int hitsOnReturn = 0;

    void Start()
    {
        timer = boomerangState.throwTime;
        startPoint = GameObject.FindGameObjectWithTag("Player").transform;
        ThrowBoomerang();
    }

    void Update()
    {
        switch (boomerangState.state)
        {
            case BoomerangLifeCycle.Thrown:
                transform.Translate(boomerangState.throwForce * Time.deltaTime * Vector3.right);
                boomerangArt.transform.Rotate(0, 0, -boomerangState.rotateForce * Time.deltaTime);
                break;

            case BoomerangLifeCycle.Returning:
                float distanceToStart = Vector3.Distance(transform.position, startPoint.position);
                float lerpValue = 1f - (distanceToStart / Vector3.Distance(transform.position, startPoint.position));
                float currentReturnForce = Mathf.Lerp(boomerangState.returnForce, 0f, lerpValue);
                transform.position = Vector3.MoveTowards(transform.position, startPoint.position, currentReturnForce * Time.deltaTime);
                transform.GetChild(0).Rotate(0, 0, boomerangState.rotateForce * Time.deltaTime);
                break;
        }

        if (transform.position == startPoint.position)
        {
            boomerangState.state = BoomerangLifeCycle.Idle;
            Destroy(this.gameObject);
        }
    }

    public void ThrowBoomerang()
    {
        boomerangState.state = BoomerangLifeCycle.Thrown;
        StartCoroutine(BoomerangTimer());
    }

    private IEnumerator BoomerangTimer()
    {
        yield return new WaitForSeconds(timer);
        boomerangState.state = BoomerangLifeCycle.Returning;
    }

    private Damage CalculateDamage()
    {
        float critChance = UnityEngine.Random.Range(0f, hitsOnReturn > 1 ? 1.5f : 1.5f + (hitsOnReturn * 0.25f));
        float critMultiplier = hitsOnReturn > 1 ? (boomerangState.critMultiplier * (1 + (hitsOnReturn * 0.075f))) : boomerangState.critMultiplier;

        bool isCrit = critChance > 1f;
        float damage;

        if (isCrit)
        {
            damage = Mathf.Floor(boomerangState.baseDamage * critChance * critMultiplier);
        }
        else
        {
            damage = Mathf.Floor(boomerangState.baseDamage);
        }

        if (timeFlowState.slowMo)
        {
            damage *= 1.33f;
        }

        return new Damage((int)damage, isCrit);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Damage damage = CalculateDamage();
            DamagePopup.Generate(collision.transform.position, damage.DamageValue, damage.IsCritical);
            collision.GetComponent<Enemy>().TakeDamage(damage.DamageValue);

            if (boomerangState.state == BoomerangLifeCycle.Returning)
            {
                hitsOnReturn++;
            }
        }
    }
}
