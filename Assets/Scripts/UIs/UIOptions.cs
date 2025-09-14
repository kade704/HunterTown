using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIOptions : MonoBehaviour
{
    private Slider _masterVolumeSlider;
    private Slider _musicVolumeSlider;
    private Slider _sfxVolumeSlider;
    private Slider _ambienceVolumeSlider;
    private Slider _cameraZoomSpeedSlider;
    private Slider _cameraMoveSpeedSlider;
    private Button _gameClearButton;
    private Button _backButton;

    private void Awake()
    {
        _masterVolumeSlider = transform.Find("Elements/Master/Slider").GetComponent<Slider>();
        _musicVolumeSlider = transform.Find("Elements/Music/Slider").GetComponent<Slider>();
        _sfxVolumeSlider = transform.Find("Elements/SFX/Slider").GetComponent<Slider>();
        _ambienceVolumeSlider = transform.Find("Elements/Ambience/Slider").GetComponent<Slider>();
        _cameraZoomSpeedSlider = transform.Find("Elements/CameraZoom/Slider").GetComponent<Slider>();
        _cameraMoveSpeedSlider = transform.Find("Elements/CameraMove/Slider").GetComponent<Slider>();
        _gameClearButton = transform.Find("Elements/GameClear/Button").GetComponent<Button>();
        _backButton = transform.Find("BackButton").GetComponent<Button>();
    }

    private void Start()
    {
        var options = GameManager.Instance.GetSystem<Options>();

        _masterVolumeSlider.value = options.MasterVolume;
        _masterVolumeSlider.onValueChanged.AddListener((value) =>
        {
            options.MasterVolume = value;
        });

        _musicVolumeSlider.value = options.MusicVolume;
        _musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            options.MusicVolume = value;
        });

        _sfxVolumeSlider.value = options.SFXVolume;
        _sfxVolumeSlider.onValueChanged.AddListener((value) =>
        {
            options.SFXVolume = value;
        });

        _ambienceVolumeSlider.value = options.AmbienceVolume;
        _ambienceVolumeSlider.onValueChanged.AddListener((value) =>
        {
            options.AmbienceVolume = value;
        });

        _cameraZoomSpeedSlider.value = options.CameraZoomSpeed;
        _cameraZoomSpeedSlider.onValueChanged.AddListener((value) =>
        {
            options.CameraZoomSpeed = value;
        });

        _cameraMoveSpeedSlider.value = options.CameraMoveSpeed;
        _cameraMoveSpeedSlider.onValueChanged.AddListener((value) =>
        {
            options.CameraMoveSpeed = value;
        });

        _gameClearButton.onClick.AddListener(() =>
        {
            options.ClearGameData();
        });

        _backButton.onClick.AddListener(() =>
        {
            options.SaveOptions();
            SceneManager.LoadScene("Menu");
        });
    }
}
