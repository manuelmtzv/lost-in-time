using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour
{
    [SerializeField] GameObject boomerangArt;
    [SerializeField] BoomerangState boomerangState;
    public TimeFlowState timeFlowState;
    public PlayerState playerState;
    private Transform startPoint;
    private float timer;
    private int hitsOnReturn = 0;
    private float baseDamage;
    private int enemiesKilledCheckpoint = 0;

    void Start()
    {
        timer = boomerangState.throwTime;
        startPoint = GameObject.FindGameObjectWithTag("Player").transform;
        baseDamage = boomerangState.baseDamage;
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
                float returnForce = boomerangState.returnForce;

                float distanceToStart = Vector3.Distance(transform.position, startPoint.position);
                float lerpValue = 1f - (distanceToStart / Vector3.Distance(transform.position, startPoint.position));
                float currentReturnForce = Mathf.Lerp(timeFlowState.slowMo ? returnForce * 1.25f : returnForce, 0f, lerpValue);
                transform.position = Vector3.MoveTowards(transform.position, startPoint.position, currentReturnForce * Time.deltaTime);
                transform.GetChild(0).Rotate(0, 0, boomerangState.rotateForce * Time.deltaTime);
                break;
        }

        if (transform.position == startPoint.position)
        {
            boomerangState.state = BoomerangLifeCycle.Idle;
            Destroy(this.gameObject);
        }

        int enemiesKilled = playerState.enemiesKilled;

        if (enemiesKilled >= enemiesKilledCheckpoint + 10)
        {
            baseDamage *= 1.025f;
            enemiesKilledCheckpoint += 10;
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
        float critChance = UnityEngine.Random.Range(0f, hitsOnReturn > 1 ? 1.6f : 1.75f + (hitsOnReturn * 0.25f));
        float critMultiplier = hitsOnReturn > 1 ? (boomerangState.critMultiplier * (1 + (hitsOnReturn * 0.075f))) : boomerangState.critMultiplier;

        bool isCrit = critChance > 1f;
        float damage;

        if (isCrit)
        {
            damage = Mathf.Floor(baseDamage * critChance * critMultiplier);
        }
        else
        {
            damage = Mathf.Floor(baseDamage);
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
            playerState.damageDealt += damage.DamageValue;

            if (boomerangState.state == BoomerangLifeCycle.Returning)
            {
                hitsOnReturn++;
            }
        }
    }
}
