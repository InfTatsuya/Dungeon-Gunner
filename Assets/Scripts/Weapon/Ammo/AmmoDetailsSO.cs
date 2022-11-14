using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AmmoDetails_", menuName ="SO/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("BASIC AMMO DETAILS")]

    [Tooltip("Ammo Name")]
    public string ammoName;

    public bool isPlayerAmmo;

    [Space(10)]
    [Header("AMMO SPRITE, PREFAB & MATERIALS")]

    [Tooltip("Ammo sprite is used for the ammo")]
    public Sprite ammoSprite;

    [Tooltip("Prefabs to be used. Can be an ammo pattern or randomly selected")]
    public GameObject[] ammoPrefabArray;

    [Tooltip("Material to be used for ammo")]
    public Material ammoMaterial;

    [Tooltip("The for charge before moving ammo pattern")]
    public float ammoChargeTime = 0.1f;

    [Tooltip("Material for charged ammo")]
    public Material ammoChargeMaterial;

    [Space(10)]
    [Header("AMMO BASE PARAMETERS")]

    [Tooltip("The damage each ammo deals")]
    public int ammoDamage = 1;

    [Tooltip("The minimum ammo speed")]
    public float ammoSpeedMin = 20f;

    [Tooltip("The maximum ammo speed")]
    public float ammoSpeedMax = 20f;

    [Tooltip("The ammo range - in unity units")]
    public float ammoRange = 20f;

    [Tooltip("The rotation for ammo pattern - degrees/seconds")]
    public float ammoRotationSpeed = 1f;

    [Space(10)]
    [Header("AMMO SPREAD DETAILS")]

    [Tooltip("The minimum spread angle of the ammo")]
    public float ammoSpreadMin = 0f;

    [Tooltip("The maximum spread angle of the ammo")]
    public float ammoSpreadMax = 0f;

    [Space(10)]
    [Header("AMMO SPAWN DETAILS")]

    [Tooltip("The minimum number of ammo spawned per shoot")]
    public int ammoSpawnAmountMin = 1;

    [Tooltip("The maximum number of ammo spawned per shoot")]
    public int ammoSpawnAmountMax = 1;

    [Tooltip("The minimum spawn interval time - seconds")]
    public float ammoSpawnIntervalMin = 0f;

    [Tooltip("The maximum spawn interval time - seconds")]
    public float ammoSpawnIntervalMax = 0f;

    [Space(10)]
    [Header("AMMO TRAILS DETAILS")]

    [Tooltip("Does ammo have trail. If selected must populate all the rest value")]
    public bool isAmmoTrail = false;

    [Tooltip("Ammo trail life time - seconds")]
    public float ammoTrailTime = 3f;

    [Tooltip("Ammo Trail material")]
    public Material ammoTrailMaterial;

    [Tooltip("The starting width for the trail")]
    [Range(0f, 1f)] public float ammoTrailStartWidth;

    [Tooltip("The ending width for the trail")]
    [Range(0f, 1f)] public float ammoTrailEndWidth;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoSprite), ammoSprite);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoPrefabArray), ammoPrefabArray);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoMaterial), ammoMaterial);

        if(ammoChargeTime > 0)
        {
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoChargeMaterial), ammoChargeMaterial);
        }

        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoDamage), ammoDamage, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpeedMin), ammoSpeedMin, nameof(ammoSpeedMax), ammoSpeedMax, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoRange), ammoRange, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpreadMin), ammoSpreadMin, nameof(ammoSpreadMax), ammoSpreadMax, true);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpawnAmountMin), ammoSpawnAmountMin, nameof(ammoSpawnAmountMax), ammoSpawnAmountMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpawnIntervalMin), ammoSpawnIntervalMin, nameof(ammoSpawnIntervalMax), ammoSpawnIntervalMax, true);

        if (isAmmoTrail)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailTime), ammoTrailTime, false);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailStartWidth), ammoTrailStartWidth, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailEndWidth), ammoTrailEndWidth, false);
        }
    }
#endif
    #endregion
}
