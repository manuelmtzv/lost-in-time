using UnityEngine;
using UnityEngine.Audio;

public class GlobalAssets : MonoBehaviour
{
    private static GlobalAssets _instance;

    public static GlobalAssets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalAssets>();
            }

            return _instance;
        }
    }

    // Prefabs
    public GameObject damagePopupPrefab;
    public GameObject sfxEmitterPrefab;

    // Audio Sources
    public AudioSource musicSource;
    public AudioDistortionFilter musicDistortionFilter;

    // Audio clips
    public AudioClip backgroundMusic;
    public AudioClip mainMenuMusic;
    public AudioClip bomerangHitSound;
    public AudioClip boomerangThrowSound;
    public AudioClip playerDamageSound;
    public AudioClip playerDeathSound;
    public AudioClip playerJumpSound;
    public AudioClip playerWalkSound;
    public AudioClip playerRunSound;
    public AudioClip playerDoubleJumpSound;
    public AudioClip enemyDeathSoundOne;
    public AudioClip enemyDeathSoundTwo;
    public AudioClip missedProyectileSound;
    public AudioClip powerUpSound;
}