using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeFlowChanger : MonoBehaviour
{
    public float slowMotionTimescale;
    public float timeAvailable;
    // public TextMeshProUGUI availableTimeText;
    public TimeFlowState timeFlowState;
    public Light2D globalLight;
    public Color slowMoColor;
    public float colorChangeDuration = 1.0f;
    public SliderBar abilityResourceBar;
    public GameState gameState;
    private float startTimescale;
    private float startFixedDeltaTime;
    private float currentTimeAvailable;
    private Color targetColor;
    private float colorChangeTimer;

    void Start()
    {
        startTimescale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        currentTimeAvailable = timeAvailable;
        timeFlowState.slowMo = false;
        targetColor = Color.white;
        globalLight.color = targetColor;

        abilityResourceBar.SetSliderMax(timeAvailable);
        abilityResourceBar.SetSliderValue(currentTimeAvailable);
    }

    void Update()
    {
        if (gameState.isPaused) return;

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

                targetColor = slowMoColor;
            }
            else
            {
                StopSlowMotion();
                targetColor = Color.white;
            }

            globalLight.intensity = 0.00001f;
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
            targetColor = Color.white;

            globalLight.intensity = 0.005f;
        }

        if (globalLight.color != targetColor)
        {
            colorChangeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(colorChangeTimer / colorChangeDuration);
            globalLight.color = Color.Lerp(globalLight.color, targetColor, t);
        }

        abilityResourceBar.SetSliderValue(currentTimeAvailable);
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
