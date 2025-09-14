using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class IntroDirector : MonoBehaviour
{
    [SerializeField] private PostProcessProfile _postProcessProfile;

    private Camera _camera;
    private Vignette _vignette;
    private ColorGrading _colorGrading;

    private void Awake()
    {
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _vignette = _postProcessProfile.GetSetting<Vignette>();
        _colorGrading = _postProcessProfile.GetSetting<ColorGrading>();
    }

    private void Start()
    {
        StartCoroutine(IntroRoutine());
    }

    private IEnumerator IntroRoutine()
    {
        _camera.orthographicSize = 10;
        _vignette.intensity.value = 0.6f;
        _colorGrading.postExposure.value = -6;

        while (3.1f <= _camera.orthographicSize)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 3, Time.deltaTime * 1f);
            _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0.3f, Time.deltaTime * 1f);
            _colorGrading.postExposure.value = Mathf.Lerp(_colorGrading.postExposure.value, 0, Time.deltaTime * 1f);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Game");
    }
}
