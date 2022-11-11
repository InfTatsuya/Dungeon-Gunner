using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MovementDetails_", menuName ="SO/Movement/Movement Details")]
public class MovementDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("MOVEMENT DETAILS")]
    [Tooltip("The minimun move speed.")]
    public float minMoveSpeed = 8f;

    [Tooltip("The maximun move speed.")]
    public float maxMoveSpeed = 8f;

    [Tooltip("The roll speed.")]
    public float rollSpeed;

    [Tooltip("The roll distance.")]
    public float rollDistance;

    [Tooltip("The roll cooldown time.")]
    public float rollCooldownTime;

    public float GetMoveSpeed()
    {
        if(minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed,
                                nameof(maxMoveSpeed), maxMoveSpeed, false);

        if(rollDistance != 0f || rollSpeed != 0f || rollCooldownTime != 0f)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollDistance), rollDistance, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollSpeed), rollSpeed, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollCooldownTime), rollCooldownTime, false);
        }
    }
#endif
    #endregion
}
