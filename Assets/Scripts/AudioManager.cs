using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("Engine")]
    public AudioSource engineSource;

    public AudioClip backgroundMusic;
    public AudioClip crashSound;
    public AudioClip fuelPickupSound;
    public AudioClip engineSound;
    public AudioClip engineLowFuelSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Criar AudioSources no próprio objeto
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        engineSource = gameObject.AddComponent<AudioSource>();

        // Configuração básica
        musicSource.loop = true;
        musicSource.volume = 0.15f;

        engineSource.loop = true;
        engineSource.volume = 1f;
    }

    private void Start()
    {
        PlayMusic();
        PlayEngine();
    }

    // MUSIC
    public void PlayMusic()
    {
        if (musicSource.isPlaying) return;

        musicSource.loop = true;
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // ENGINE NORMAL
    public void PlayEngine()
    {
        engineSource.clip = engineSound;
        engineSource.loop = true;
        engineSource.Play();
    }

    // ENGINE LOW FUEL
    public void PlayLowFuelEngine()
    {
        if (engineSource.clip == engineLowFuelSound) return;

        engineSource.clip = engineLowFuelSound;
        engineSource.volume = 0.4f;
        engineSource.Play();
    }

    public void PlayNormalEngine()
    {
        if (engineSource.clip == engineSound) return;

        engineSource.clip = engineSound;
        engineSource.Play();
    }
    public void StopEngine()
    {
        engineSource.Stop();
    }

    // SFX
    public void PlayCrash()
    {
        sfxSource.PlayOneShot(crashSound);
    }

    public void PlayFuelPickup()
    {
        sfxSource.PlayOneShot(fuelPickupSound);
    }
}