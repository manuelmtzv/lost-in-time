using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeFlowChanger : MonoBehaviour
{
    public float slowMotionTimescale;
    public float timeAvailable;
    public TextMeshProUGUI availableTimeText;
    public TimeFlowState timeFlowState;
    private float startTimescale;
    private float startFixedDeltaTime;
    private float currentTimeAvailable;

    void Start()
    {
        startTimescale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        currentTimeAvailable = timeAvailable;

        timeFlowState.slowMo = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            timeFlowState.slowMo = !timeFlowState.slowMo;
        }

        if (timeFlowState.slowMo)
        {
            if (currentTimeAvailable > 0)
            {
                StartSlowMotion();
                currentTimeAvailable -= Time.deltaTime;

                if (currentTimeAvailable < 0) currentTimeAvailable = 0;
            }
            else
            {
                StopSlowMotion();
            }
        }
        else
        {
            if (currentTimeAvailable < timeAvailable)
            {
                currentTimeAvailable += Time.deltaTime / 3.5f;
            }
            else
            {
                currentTimeAvailable = timeAvailable;
            }
            StopSlowMotion();
        }

        availableTimeText.text = "Time available: " + currentTimeAvailable.ToString("F2");
    }

    private void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimescale;
    }

    private void StopSlowMotion()
    {
        Time.timeScale = startTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime;
        timeFlowState.slowMo = false;
    }
}
