using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _ambienceVolumeSlider;
    [SerializeField] private Button _removeSaveButton;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        var audioMixer = GameManager.Instance.GetSystem<AudioController>();

        _masterVolumeSlider.value = audioMixer.MasterVolume;
        _musicVolumeSlider.value = audioMixer.MusicVolume;
        _sfxVolumeSlider.value = audioMixer.SFXVolume;
        _ambienceVolumeSlider.value = audioMixer.AmbienceVolume;

        _masterVolumeSlider.onValueChanged.AddListener((value) => audioMixer.MasterVolume = value);
        _musicVolumeSlider.onValueChanged.AddListener((value) => audioMixer.MusicVolume = value);
        _sfxVolumeSlider.onValueChanged.AddListener((value) => audioMixer.SFXVolume = value);
        _ambienceVolumeSlider.onValueChanged.AddListener((value) => audioMixer.AmbienceVolume = value);
        _removeSaveButton.onClick.AddListener(() =>
        {
            if (File.Exists(Application.persistentDataPath + "/SaveData1.json"))
                File.Delete(Application.persistentDataPath + "/SaveData1.json");

            if (File.Exists(Application.persistentDataPath + "/SaveData2.json"))
                File.Delete(Application.persistentDataPath + "/SaveData2.json");

            if (File.Exists(Application.persistentDataPath + "/SaveData3.json"))
                File.Delete(Application.persistentDataPath + "/SaveData3.json");
        });
        _backButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SaveOptions();
            GameManager.Instance.GoMenu();
        });
    }
}
