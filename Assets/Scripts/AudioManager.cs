using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music, sfx;
    public TimeFlowState timeFlowState;

    public static AudioManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        music.clip = clip;
        music.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, bool affectedByTimeEffect = true)
    {
        GameObject sfxEmitter = Instantiate(GlobalAssets.Instance.sfxEmitterPrefab, transform.position, Quaternion.identity);
        AudioSource audioSource = sfxEmitter.GetComponent<AudioSource>();

        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);

        if (affectedByTimeEffect)
        {
            audioSource.pitch = timeFlowState.slowMo ? 0.5f : 1;
        }

        Destroy(sfxEmitter, clip.length);
    }

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
