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
    public const string MIXER_MUSIC = "musicVol";
    public const string MIXER_SFX = "sfxVol";

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }
    public void MusicVolume (float volume)
    {
        mixer.SetFloat(MIXER_MUSIC, volume);
    }

    public void SFXVolume (float volume)
    {
        mixer.SetFloat(MIXER_SFX, volume);
    }
}
