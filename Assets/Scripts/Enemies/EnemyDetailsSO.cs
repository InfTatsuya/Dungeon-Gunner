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

    [Space(10)]
    [Header("ENEMY WEAPON SETTINGS")]

    [Tooltip("The weapon for enemy - none if enemy doesn't have weapon")]
    public WeaponDetailsSO enemyWeapon;

    [Tooltip("The minimum time delay interval in seconds between bursts of enemy shooting.")]
    public float firingIntervalMin = 0.1f;

    [Tooltip("The maximum time delay interval in seconds between bursts of enemy shooting.")]
    public float firingIntervalMax = 1f;

    [Tooltip("The minimum firing duration that enemy shoots during a firing burst.")]
    public float firingDurationlMin = 1f;

    [Tooltip("The maximum firing duration that enemy shoots during a firing burst.")]
    public float firingDurationlMax = 2f;

    [Tooltip("Is enemy need line of sight before shoot")]
    public bool firingLineOfSightRequired;

    [Space(10)]
    [Header("ENEMY HEALTH")]

    [Tooltip("The health of enemy for each level")]
    public EnemyHealthDetails[] enemyHealthDetailsArray;

    [Tooltip("Select if has immunity period after being hit")]
    public bool isImmunityAfterHit = false;

    [Tooltip("Immunity time - seconds")]
    public float hitImmunityTime;

    [Tooltip("Select to display a health bar for the enemy")]
    public bool isHealthBardisplayed = false;

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
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin,
            nameof(firingIntervalMax), firingIntervalMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationlMin), firingDurationlMin,
            nameof(firingDurationlMax), firingDurationlMax, false);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyHealthDetailsArray), enemyHealthDetailsArray);
        if (isImmunityAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }
    }
#endif
    #endregion
}
