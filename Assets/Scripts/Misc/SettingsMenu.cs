using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    public const string MUSIC_KEY = "musicVol";
    public const string SFX_KEY = "sfxVol";

    private void Start()
    {
        LoadVolume();
    }

    void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        mixer.SetFloat(MUSIC_KEY, musicVol);
        mixer.SetFloat(SFX_KEY, sfxVol);
        musicSlider.value = Mathf.Pow(10, musicVol / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVol / 20);
    }

    public void SetMusicVolume()
    {
        mixer.SetFloat(MUSIC_KEY, convertVolume(musicSlider.value));
        PlayerPrefs.SetFloat(MUSIC_KEY, convertVolume(musicSlider.value));
    }

    public void SetSFXVolume()
    {
        mixer.SetFloat(SFX_KEY, convertVolume(sfxSlider.value));
        PlayerPrefs.SetFloat(SFX_KEY, convertVolume(sfxSlider.value));
    }

    float convertVolume(float input)
    {
        return Mathf.Log10(input) * 20;
    }
}
