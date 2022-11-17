using UnityEngine;

[CreateAssetMenu(fileName ="WeaponShootEffect_", menuName ="SO/Weapons/Weapon Shoot Effect")]
public class WeaponShootEffectSO : ScriptableObject
{
    [Space(10)]
    [Header("WEAPON SHOOT EFFECT DETAILS")]

    [Tooltip("The color gradient for shoot effect")]
    public Gradient colorGradient;

    [Tooltip("The duration of shoot effect")]
    public float duration = 0.5f;

    [Tooltip("The start particle size of effect")]
    public float startParticleSize = 0.25f;

    [Tooltip("The start particle speed of effect")]
    public float startParticleSpeed = 3f;

    [Tooltip("The particle lifetime of effect")]
    public float startLifeTime = 0.5f;

    [Tooltip("The maximum number of particles for effect")]
    public int maxParticleNumber = 100;

    [Tooltip("The number of particles emitted per second. If zero it will be the burst number")]
    public int emissionRate = 100;

    [Tooltip("The number of particles per burst emitting")]
    public int burstParticleNumber = 20;

    [Tooltip("The gravity on the particles - must be a small negative number")]
    public float effectGravity = -0.01f;

    [Tooltip("the sprite for the particle effect")]
    public Sprite sprite;

    [Tooltip("The minimum velocity over lifetime")]
    public Vector3 velocityOverLifetimeMin;

    [Tooltip("The maximum velocity over lifetime")]
    public Vector3 velocityOverLifetimeMax;

    [Tooltip("The prefab of WeaponShootEffect")]
    public GameObject weaponShootEffectPrefab;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(duration), duration, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSize), startParticleSize, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startLifeTime), startLifeTime, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(emissionRate), emissionRate, true);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootEffectPrefab), weaponShootEffectPrefab);
    }
#endif
    #endregion
}
