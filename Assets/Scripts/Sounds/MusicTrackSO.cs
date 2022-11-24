using UnityEngine;

[CreateAssetMenu(fileName ="MusicTrack_", menuName ="SO/Sounds/Music Track")]
public class MusicTrackSO : ScriptableObject
{
    [Space(10)]
    [Header("MUSIC TRACK DETAILS")]

    [Tooltip("The name of the music track")]
    public string musicName;

    [Tooltip("The audio clip for the music track")]
    public AudioClip musicClip;

    [Tooltip("the volume for the music track")]
    public float musicVolume = 1f;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(musicName), musicName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicClip), musicClip);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(musicVolume), musicVolume, true);    
    }
#endif
    #endregion
}
