using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioMixer mixer;

    //public Sound[] musicSound, sfxSounds;
    //public AudioSource musicSource, sfxSource;
    //[SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> jumpClips = new List<AudioClip>();
    public const string MUSIC_KEY = "musicVol";
    public const string SFX_KEY = "sfxVol";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();
    }

    void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        mixer.SetFloat(SettingsMenu.MIXER_MUSIC, musicVol);
        mixer.SetFloat(SettingsMenu.MIXER_SFX, sfxVol);
    }

    public void jumpSFX()
    {

    }
}
