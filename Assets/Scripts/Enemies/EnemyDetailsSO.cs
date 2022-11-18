using UnityEngine;

[CreateAssetMenu(fileName ="EnemyDetails_", menuName ="SO/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("BASE ENEMY DETAILS")]

    [Tooltip("The name of enemy")]
    public string enemyName;

    [Tooltip("The prefab for the enemy")]
    public GameObject enemyPrefab;

    [Tooltip("Distance to the player before enemy starts chasing")]
    public float chaseDistance = 50f;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);
    }
#endif
    #endregion
}
