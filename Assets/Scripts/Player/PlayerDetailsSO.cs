using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerDetails_", menuName = "SO/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("PLAYER BASE DETAILS"), Tooltip("Player character name.")]
    public string playerCharacterName;

    [Tooltip("Prefab for the player")]
    public GameObject playerPrefab;

    public RuntimeAnimatorController runtimeAnimatorController;

    [Space(10)]
    [Header("HEALTH"), Tooltip("Player starting health amount")]
    public int playerHealthAmount;

    [Space(10)]
    [Header("OTHER"), Tooltip("Player icon sprite to be used in the minimap")]
    public Sprite playerMinimapIcon;

    [Tooltip("Player hand sprite")]
    public Sprite playerHandSprite;

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);

        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);

        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);

        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMinimapIcon), playerMinimapIcon);

        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);

        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);

    }

#endif

    #endregion

}