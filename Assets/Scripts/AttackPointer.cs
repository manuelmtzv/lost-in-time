using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] GameObject pointer;
    [SerializeField] GameObject boomerang;
    [SerializeField] BoomerangState boomerangState;

    private void Start()
    {
        this.gameObject.SetActive(true);
        boomerangState.state = BoomerangLifeCycle.Idle;
    }

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0))
        {
            if (boomerangState.state == BoomerangLifeCycle.Idle)
            {
                Instantiate(boomerang, pointer.transform.position, pointer.transform.rotation);
            }
        }
    }

    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

        Vector3 difference = mousePosition - transform.position;

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}

