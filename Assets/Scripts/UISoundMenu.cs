using UnityEngine;
using UnityEngine.UI;

public class UISoundMenu : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicVolumeSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        sfxVolumeSlider = GameObject.Find("SFX Slider").GetComponent<Slider>();

        musicVolumeSlider.value = AudioManager.Instance.musicSource.volume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxSource.volume;

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }
    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }
    public void CloseVolumePanel()
    {
        GameObject.Find("Volume Panel").SetActive(false);
    }
}
