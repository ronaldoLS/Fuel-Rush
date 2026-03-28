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

    public float MusicVolume { get; private set; }
    public float SoundEffectsVolume { get; private set; }


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
        MusicVolume = 0.60f;
        SoundEffectsVolume = 1f;

        musicSource.loop = true;
        musicSource.volume = MusicVolume;

        engineSource.loop = true;
        engineSource.volume = SoundEffectsVolume;

        
    }

    private void Start()
    {        
        PlayMusic();
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
        if (engineSource.isPlaying)
            return;

        engineSource.clip = engineSound;
        engineSource.loop = true;
        engineSource.Play();
    }

    // ENGINE LOW FUEL
    public void PlayLowFuelEngine()
    {
        if (engineSource.clip == engineLowFuelSound) return;

        engineSource.clip = engineLowFuelSound;
        engineSource.volume = SoundEffectsVolume * 0.2f;
        engineSource.Play();
    }

    public void PlayNormalEngine()
    {
        if (engineSource.clip == engineSound) return;
        engineSource.clip = engineSound;
        engineSource.volume = SoundEffectsVolume;
        engineSource.Play();
    }
    public void StopEngineSound()
    {
        engineSource.Stop();
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume * 0.75f;
        engineSource.volume = volume;
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