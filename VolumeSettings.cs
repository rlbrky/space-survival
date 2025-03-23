using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public static VolumeSettings instance;

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Pause Menu")]
    [SerializeField] private GameObject muteImageEffect;
    [SerializeField] private GameObject muteImageEffectSFX;

    private bool bgMusicMuted;
    private bool SFXMuted;

    private float mutedMusicSliderValue = 0.2f;
    private float mutedSFXSliderValue = 0.2f;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("isMusicMuted") && PlayerPrefs.HasKey("isSFXMuted"))
        {
            LoadMuteSettings();
            RevertMuteEffects();
        }

        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
            LoadVolume();
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    private void OnEnable()
    {
        if (PlayerTouchInputs.instance != null)
        {
            PlayerTouchInputs.instance.DisableTouchStuff();
            PlayerTouchInputs.instance.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (PlayerTouchInputs.instance != null)
        {
            PlayerTouchInputs.instance.EnableTouchStuff();
            PlayerTouchInputs.instance.gameObject.SetActive(true);
        }
    }

    public void SetMusicVolume()
    {
        myMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        if(bgMusicMuted)
            MuteMusic();
    }

    public void SetSFXVolume()
    {
        myMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        if (SFXMuted)
            MuteSFX();
    }

    public void MuteMusic()
    {
        if (bgMusicMuted)
        {
            bgMusicMuted = false;
            musicSlider.value = mutedMusicSliderValue;
            SetMusicVolume();
        }
        else //Mute music.
        {
            mutedMusicSliderValue = musicSlider.value;
            musicSlider.value = 0.0001f;
            SetMusicVolume();
            bgMusicMuted = true;
        }

        muteImageEffect.SetActive(bgMusicMuted);
        PlayerPrefs.SetInt("isMusicMuted", bgMusicMuted ? 1 : 0);
    }

    public void MuteSFX()
    {
        if (SFXMuted)
        {
            SFXMuted = false;
            sfxSlider.value = mutedSFXSliderValue;
            SetSFXVolume();
        }
        else //MuteSFX
        {
            mutedSFXSliderValue = sfxSlider.value;
            sfxSlider.value = 0.0001f;
            SetSFXVolume();
            SFXMuted = true;
        }

        muteImageEffectSFX.SetActive(SFXMuted);
        PlayerPrefs.SetInt("isSFXMuted", SFXMuted ? 1 : 0);
    }

    public void RevertMuteEffects()
    {
        musicSlider.value = mutedMusicSliderValue;
        SetMusicVolume();
        sfxSlider.value = mutedSFXSliderValue;
        SetSFXVolume();
    }

    public void PlayAssignedSFX(AudioSource audioSource)
    {
        audioSource.Play();
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetSFXVolume();
        SetMusicVolume();
    }

    private void LoadMuteSettings()
    {
        if(PlayerPrefs.GetInt("isMusicMuted") == 0)
        {
            muteImageEffect.SetActive(true);
            bgMusicMuted = true;
        }
        else
        {
            muteImageEffect.SetActive(false);
            bgMusicMuted = false;
        }

        if (PlayerPrefs.GetInt("isSFXMuted") == 0)
        {
            muteImageEffectSFX.SetActive(true);
            SFXMuted = true;
        }
        else
        {
            muteImageEffectSFX.SetActive(false);
            SFXMuted = false;
        }
    }
}
