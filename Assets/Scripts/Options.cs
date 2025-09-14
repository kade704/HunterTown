using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Options : MonoBehaviour
{
    private float _masterVolume = 1.0f;
    private float _musicVolume = 1.0f;
    private float _sfxVolume = 1.0f;
    private float _ambienceVolume = 1.0f;
    private float _cameraZoomSpeed = 23f;
    private float _cameraMoveSpeed = 8f;

    private UnityEvent<float> _onMasterVolumeChanged = new();
    private UnityEvent<float> _onMusicVolumeChanged = new();
    private UnityEvent<float> _onSFXVolumeChanged = new();
    private UnityEvent<float> _onAmbienceVolumeChanged = new();
    private UnityEvent<float> _onCameraZoomSpeedChanged = new();
    private UnityEvent<float> _onCameraMoveSpeedChanged = new();

    public UnityEvent<float> OnMasterVolumeChanged => _onMasterVolumeChanged;
    public UnityEvent<float> OnMusicVolumeChanged => _onMusicVolumeChanged;
    public UnityEvent<float> OnSFXVolumeChanged => _onSFXVolumeChanged;
    public UnityEvent<float> OnAmbienceVolumeChanged => _onAmbienceVolumeChanged;
    public UnityEvent<float> OnCameraZoomSpeedChanged => _onCameraZoomSpeedChanged;
    public UnityEvent<float> OnCameraMoveSpeedChanged => _onCameraMoveSpeedChanged;

    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            _onMasterVolumeChanged.Invoke(value);
        }
    }

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            _onMusicVolumeChanged.Invoke(value);
        }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            _onSFXVolumeChanged.Invoke(value);
        }
    }

    public float AmbienceVolume
    {
        get => _ambienceVolume;
        set
        {
            _ambienceVolume = value;
            _onAmbienceVolumeChanged.Invoke(value);
        }
    }

    public float CameraZoomSpeed
    {
        get => _cameraZoomSpeed;
        set
        {
            _cameraZoomSpeed = value;
            _onCameraZoomSpeedChanged.Invoke(value);
        }
    }

    public float CameraMoveSpeed
    {
        get => _cameraMoveSpeed;
        set
        {
            _cameraMoveSpeed = value;
            _onCameraMoveSpeedChanged.Invoke(value);
        }
    }

    private void Start()
    {
        LoadOptions();
    }

    public void ClearGameData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData1.json"))
            File.Delete(Application.persistentDataPath + "/SaveData1.json");

        if (File.Exists(Application.persistentDataPath + "/SaveData2.json"))
            File.Delete(Application.persistentDataPath + "/SaveData2.json");

        if (File.Exists(Application.persistentDataPath + "/SaveData3.json"))
            File.Delete(Application.persistentDataPath + "/SaveData3.json");
    }

    public void SaveOptions()
    {
        var parser = new INIParser();
        parser.Open(Application.persistentDataPath + "/Options.ini");
        parser.WriteValue("Audio", "MasterVolume", _masterVolume.ToString());
        parser.WriteValue("Audio", "MusicVolume", _musicVolume.ToString());
        parser.WriteValue("Audio", "SFXVolume", _sfxVolume.ToString());
        parser.WriteValue("Audio", "AmbienceVolume", _ambienceVolume.ToString());
        parser.WriteValue("Game", "CameraMoveSpeed", _cameraMoveSpeed.ToString());
        parser.WriteValue("Game", "CameraZoomSpeed", _cameraZoomSpeed.ToString());
        parser.Close();

        Debug.Log("Options saved.");
    }

    public void LoadOptions()
    {
        var parser = new INIParser();
        parser.Open(Application.persistentDataPath + "/Options.ini");
        _masterVolume = float.Parse(parser.ReadValue("Audio", "MasterVolume", "1.0"));
        _musicVolume = float.Parse(parser.ReadValue("Audio", "MusicVolume", "1.0"));
        _sfxVolume = float.Parse(parser.ReadValue("Audio", "SFXVolume", "1.0"));
        _ambienceVolume = float.Parse(parser.ReadValue("Audio", "AmbienceVolume", "1.0"));
        _cameraMoveSpeed = float.Parse(parser.ReadValue("Game", "CameraMoveSpeed", "8.0"));
        _cameraZoomSpeed = float.Parse(parser.ReadValue("Game", "CameraZoomSpeed", "23.0"));
        parser.Close();

        Debug.Log("Options loaded.");
    }
}
