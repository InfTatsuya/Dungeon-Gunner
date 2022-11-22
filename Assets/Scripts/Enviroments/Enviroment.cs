using UnityEngine;

[DisallowMultipleComponent]
public class Enviroment : MonoBehaviour
{
    [Space(10)]
    [Header("REFERENCES")]
    [Tooltip("The SpriteRenderer component on the prefab")]
    public SpriteRenderer spriteRenderer;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(spriteRenderer), spriteRenderer);
    }
#endif
    #endregion
}
