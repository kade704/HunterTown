using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _musicClips;
    [SerializeField] private AudioClip[] _ambienceClips;
    [SerializeField] private AudioClip[] _sfxClips;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _ambienceSource;

    private float _masterVolume = 0;
    private float _musicVolume = 0;
    private float _sfxVolume = 0;
    private float _ambienceVolume = 0;

    private int _currentMusicIndex = 0;


    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            SetGroupVolume("MasterVolume", value);
        }
    }

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            SetGroupVolume("MusicVolume", value);
        }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            SetGroupVolume("SFXVolume", value);
        }
    }

    public float AmbienceVolume
    {
        get => _ambienceVolume;
        set
        {
            _ambienceVolume = value;
            SetGroupVolume("AmbienceVolume", value);
        }
    }


    public float MasterCutoff
    {
        set => _audioMixer.SetFloat("MasterCutoff", value * 22000f);
    }

    private void Start()
    {
        var options = GameManager.Instance.GetSystem<Options>();

        MasterVolume = options.MasterVolume;
        MusicVolume = options.MusicVolume;
        SFXVolume = options.SFXVolume;
        AmbienceVolume = options.AmbienceVolume;

        options.OnMasterVolumeChanged.AddListener((value) => MasterVolume = value);
        options.OnMusicVolumeChanged.AddListener((value) => MusicVolume = value);
        options.OnSFXVolumeChanged.AddListener((value) => SFXVolume = value);
        options.OnAmbienceVolumeChanged.AddListener((value) => AmbienceVolume = value);

        StartCoroutine(MusicRoutine());
    }

    private void SetGroupVolume(string groupName, float volume)
    {
        _audioMixer.SetFloat(groupName, volume > 0 ? Mathf.Log10(volume) * 20 : -80);
    }

    private IEnumerator MusicRoutine()
    {
        while (true)
        {
            if (!_musicSource.isPlaying)
            {
                PlayNextMusic();
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void PlayNextMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = _musicClips[_currentMusicIndex++ % _musicClips.Length];
        _musicSource.Play();
    }

    public void PlaySFX(string clipName)
    {
        var clip = Array.Find(_sfxClips, c => c.name == clipName);
        if (clip == null)
        {
            Debug.LogError($"SFX clip {clipName} not found");
            return;
        }

        _sfxSource.PlayOneShot(clip);
    }

    public void PlayAmbience(string clipName)
    {
        var clip = Array.Find(_ambienceClips, c => c.name == clipName);
        if (clip == null)
        {
            Debug.LogError($"Ambience clip {clipName} not found");
            return;
        }

        _ambienceSource.clip = clip;
        _ambienceSource.loop = true;
        _ambienceSource.Play();
    }

    public void StopAmbience()
    {
        _ambienceSource.Stop();
    }
}
