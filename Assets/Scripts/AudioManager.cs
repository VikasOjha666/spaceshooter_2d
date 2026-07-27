using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip meteorExplosionClip;
    public AudioClip shipExplosionClip;
    public AudioClip backgroundMusicClip;
    [Range(0f, 1f)]
    public float backgroundMusicVolume = 0.15f;

    AudioSource musicSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Preload all explosion audio at game start
        if (meteorExplosionClip != null)
            meteorExplosionClip.LoadAudioData();

        if (shipExplosionClip != null)
            shipExplosionClip.LoadAudioData();
    }

    void Start()
    {
        StartBackgroundMusic();
    }

    void StartBackgroundMusic()
    {
        if (backgroundMusicClip == null)
            return;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusicClip;
        musicSource.loop = true;
        musicSource.volume = backgroundMusicVolume;
        musicSource.playOnAwake = false;
        musicSource.Play();
    }

    public static void PlayMeteorExplosion(Vector3 position)
    {
        if (Instance != null && Instance.meteorExplosionClip != null)
        {
            AudioSource.PlayClipAtPoint(Instance.meteorExplosionClip, position);
        }
    }

    public static void PlayShipExplosion(Vector3 position)
    {
        if (Instance != null && Instance.shipExplosionClip != null)
        {
            AudioSource.PlayClipAtPoint(Instance.shipExplosionClip, position);
        }
    }
}
