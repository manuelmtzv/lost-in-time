using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFlowTransition : MonoBehaviour
{
    public TimeFlowState timeFlowState;
    private AudioSource musicSource;
    private AudioDistortionFilter audioEchoFilter;

    void Start()
    {
        musicSource = GlobalAssets.Instance.musicSource;
        audioEchoFilter = GlobalAssets.Instance.musicDistortionFilter;
    }

    void Update()
    {
        if (timeFlowState.slowMo)
        {
            if (musicSource.pitch != 0.5f)
            {
                StartCoroutine(SmoothEffectTransition(0.5f, 0.5f, 0.4f));
            }
        }
        else
        {
            if (musicSource.pitch != 1.0f)
            {
                StartCoroutine(SmoothEffectTransition(1.0f, 0f, 0.4f));
            }
        }
    }

    IEnumerator SmoothEffectTransition(float targetPitch, float targetDistortion, float duration)
    {
        float startPitch = musicSource.pitch;
        float startDistortion = audioEchoFilter.distortionLevel;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentPitch = Mathf.Lerp(startPitch, targetPitch, elapsedTime / duration);
            float currentDistortion = Mathf.Lerp(startDistortion, targetDistortion, elapsedTime / duration);
            musicSource.pitch = currentPitch;
            audioEchoFilter.distortionLevel = currentDistortion;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        musicSource.pitch = targetPitch;
    }


}
