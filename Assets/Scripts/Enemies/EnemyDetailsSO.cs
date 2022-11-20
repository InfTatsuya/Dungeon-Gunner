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

    [Space(10)]
    [Header("ENEMY MATERIALS")]

    [Tooltip("The standard lit shader material for enemy")]
    public Material enemyStandardMaterial;

    [Space(10)]
    [Header("ENEMY MATERIALIZE SETTINGS")]

    [Tooltip("The time in seconds takes to materialize the enemy")]
    public float enemyMaterializeTime;

    [Tooltip("The shader is used when apply effect")]
    public Shader enemyMaterializeShader;

    [ColorUsage(true, true)]
    [Tooltip("The colour used when enemy materializes.")]
    public Color enemyMaterializeColor;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyStandardMaterial), enemyStandardMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(enemyMaterializeTime), enemyMaterializeTime, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyMaterializeShader), enemyMaterializeShader);
    }
#endif
    #endregion
}
