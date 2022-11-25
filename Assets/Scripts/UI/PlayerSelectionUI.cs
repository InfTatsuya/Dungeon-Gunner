using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSelectionUI : MonoBehaviour
{
    [Tooltip("The SpriteRenderer on child gameobject WeaponAnchorPostion/WeaponRotationPoint/Hand")]
    public SpriteRenderer playerHandSpriteRenderer;

    [Tooltip("The SpriteRenderer on child gameobject HandNoWeapon")]
    public SpriteRenderer playerHandNoWeaponSpriteRenderer;

    [Tooltip("The SpriteRenderer on child gameobject WeaponAnchorPostion/WeaponRotationPoint/Weapon")]
    public SpriteRenderer playerWeaponSpriteRenderer;

    [Tooltip("The Animator component")]
    public Animator animator;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSpriteRenderer), playerHandSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandNoWeaponSpriteRenderer), playerHandNoWeaponSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerWeaponSpriteRenderer), playerWeaponSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(animator), animator);
    }
#endif
    #endregion
}
