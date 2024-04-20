using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public void Start()
    {
        DamagePopup.Generate(new Vector3(0, 0, 0), 100);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool randomBool = Random.value > 0.5f;

            DamagePopup.Generate(new Vector3(0, 0, 0), 100, randomBool);
        }
    }
}
