using UnityEngine;

[CreateAssetMenu(fileName ="SoundEffect_", menuName ="SO/Sounds/Sound Effect")]
public class SoundEffectSO : ScriptableObject
{

    [Space(10)]
    [Header("SOUND EFFECT DETAILS")]

    [Tooltip("The name of sound effect")]
    public string soundEffectName;

    [Tooltip("The prefab for sound effect")]
    public GameObject soundPrefab;

    [Tooltip("The Audio Clip for sound effect")]
    public AudioClip soundEffectClip;

    [Tooltip("The minimum pitch variation for sound effect.")]
    [Range(0.1f, 1.5f)] public float soundEffectPitchRandomVariationMin = 0.8f;

    [Tooltip("The maximum pitch variation for sound effect.")]
    [Range(0.1f, 1.5f)] public float soundEffectPitchRandomVariationMax = 1.2f;

    [Tooltip("Sound effect volume")]
    [Range(0.0f, 1.0f)] public float soundEffectVolume = 1.0f;

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(soundEffectName), soundEffectName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundPrefab), soundPrefab);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundEffectClip), soundEffectClip);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(soundEffectPitchRandomVariationMin), soundEffectPitchRandomVariationMin,
                            nameof(soundEffectPitchRandomVariationMax), soundEffectPitchRandomVariationMax, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(soundEffectVolume), soundEffectVolume, true);
    }
#endif
    #endregion
}
