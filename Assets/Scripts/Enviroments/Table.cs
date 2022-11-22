using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Table : MonoBehaviour, IUseable
{
    [Tooltip("The mass of the table to control the speed when pushed")]
    [SerializeField] private float itemMass;

    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb2D;
    private bool isUse = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void UseItem()
    {
        if (!isUse)
        {
            Bounds bounds = boxCollider2D.bounds;

            Vector3 closesPointToPlayer = bounds.ClosestPoint(GameManager.Instance.GetPlayer().GetPlayerPosition());

            if(closesPointToPlayer.x == bounds.max.x)
            {
                animator.SetBool(Settings.flipLeft, true);
            }
            else if(closesPointToPlayer.x == bounds.min.x)
            {
                animator.SetBool(Settings.flipRight, true);
            }
            else if (closesPointToPlayer.y == bounds.min.y)
            {
                animator.SetBool(Settings.flipUp, true);
            }
            else
            {
                animator.SetBool(Settings.flipDown, true);
            }

            gameObject.layer = LayerMask.NameToLayer("Environment");
            rb2D.mass = itemMass;

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.tableFlipSoundEffect);

            isUse = true;
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(itemMass), itemMass, false);
    }
#endif
    #endregion
}
