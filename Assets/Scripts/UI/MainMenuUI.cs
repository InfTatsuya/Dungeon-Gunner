using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Space(10)]
    [Header("OBJECT REFERENCES")]

    [Tooltip("The PlayGame button gameobject")]
    [SerializeField] private GameObject playButton;

    [Tooltip("The Quit button gameobject")]
    [SerializeField] private GameObject quitButton;

    [Tooltip("The HighScores button gameobject")]
    [SerializeField] private GameObject highScoresButton;

    [Tooltip("The Instructions button gameobject")]
    [SerializeField] private GameObject instructionsButton;

    [Tooltip("The ReturnToMainMenu button gameobject")]
    [SerializeField] private GameObject returnToMainMenuButton;

    private bool isHighScoresSceneLoaded = false;
    private bool isInstructionsSceneLoaded = false;

    void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        SceneManager.LoadScene(2, LoadSceneMode.Additive);

        returnToMainMenuButton.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadHighScores()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        instructionsButton.SetActive(false);
        isHighScoresSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");

        returnToMainMenuButton.SetActive(true);

        SceneManager.LoadScene("HighScoresScene", LoadSceneMode.Additive);
    }

    public void LoadInstructions()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        instructionsButton.SetActive(false);
        isInstructionsSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");

        returnToMainMenuButton.SetActive(true);

        SceneManager.LoadScene("InstructionScene", LoadSceneMode.Additive);
    }

    public void LoadCharacterSelector()
    {
        returnToMainMenuButton.SetActive(false);

        if (isHighScoresSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("HighScoresScene");
            isHighScoresSceneLoaded = false;
        }
        else if (isInstructionsSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("InstructionScene");
            isInstructionsSceneLoaded = false;
        }

        playButton.SetActive(true);
        quitButton.SetActive(true);
        highScoresButton.SetActive(true);
        instructionsButton.SetActive(true);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(playButton), playButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(instructionsButton), instructionsButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(highScoresButton), highScoresButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(returnToMainMenuButton), returnToMainMenuButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(quitButton), quitButton);
    }
#endif
    #endregion
}
