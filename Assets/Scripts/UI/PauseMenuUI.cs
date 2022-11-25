using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    [Tooltip("Populate with the music volume level")]
    [SerializeField] private TextMeshProUGUI musicLevelText;

    [Tooltip("Populate with the sound volume level")]
    [SerializeField] private TextMeshProUGUI soundLevelText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator InitializeUI()
    {
        yield return null; // delay a frame to ensure the previous value of music and sound volume have been set

        soundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        StartCoroutine(InitializeUI());
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseSoundVolume()
    {
        SoundEffectManager.Instance.IncreaseSoundVolume();
        soundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void DecreaseSoundVolume()
    {
        SoundEffectManager.Instance.DecreaseSoundVolume();
        soundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundLevelText), soundLevelText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicLevelText), musicLevelText);
    }
#endif
    #endregion
}
