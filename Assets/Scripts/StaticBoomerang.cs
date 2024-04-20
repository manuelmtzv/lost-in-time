using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBoomerang : MonoBehaviour
{
    [SerializeField] BoomerangState boomerangState;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.enabled = boomerangState.state == BoomerangLifeCycle.Idle;
    }
}
