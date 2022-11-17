using UnityEngine;

[CreateAssetMenu(fileName ="WeaponDetails_", menuName ="SO/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]

    [Tooltip("Weapon Name")]
    public string weaponName;

    [Tooltip("The sprite for the weapon - must have the 'generate physics shape' options ticked")]
    public Sprite weaponSprite;

    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    [Tooltip("Weapon Shoot Position - offset position from the sprite pivot point.")]
    public Vector3 weaponShootPosition;

    [Tooltip("Weapon ammo type")]
    public AmmoDetailsSO weaponCurrentAmmo;

    [Tooltip("The weapon shoot effect")]
    public WeaponShootEffectSO weaponShootEffect;

    [Tooltip("The firing sound effect SO for the weapon")]
    public SoundEffectSO weaponFiringSoundEffect;

    [Tooltip("The reloading sound effect SO for the weapon")]
    public SoundEffectSO weaponReloadingSoundEffect;

    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]

    [Tooltip("Does weapon have infinite ammo")]
    public bool hasInfiniteAmmo = false;

    [Tooltip("Does weapon have infinite clip capacity")]
    public bool hasInfiniteClipCapacity = false;

    [Tooltip("Weapon clip capacity")]
    public int weaponClipAmmoCapacity = 6;

    [Tooltip("Weapon ammo capacity - the maximun ammo can be held for this weapon")]
    public int weaponAmmoCapacity = 100;

    [Tooltip("Weapon Fire Rate - seconds/shoot")]
    public float weaponFireRate = 0.2f;

    [Tooltip("Weapon Precharge time - time for charge weapon before firing")]
    public float weaponPrechargeTime = 0f;

    [Tooltip("Weapon Reload time - seconds")]
    public float weaponReloadTime = 0f;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false); 
        }

        if (!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }
#endif
    #endregion
}
