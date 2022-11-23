using System.Collections;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]
public class Chest : MonoBehaviour, IUseable
{
    [Tooltip("The color is used for the materialization effect - HDR, not show Alpha")]
    [ColorUsage(false, true)]
    [SerializeField] private Color materializeColor;

    [Tooltip("The time for materialization effect")]
    [SerializeField] private float materializeTime = 3f;

    [Tooltip("The ItemSpawnPoint transform")]
    [SerializeField] private Transform itemSpawnPoint;

    private int healthPercent;
    private WeaponDetailsSO weaponDetails;
    private int ammoPercent;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MaterializeEffect materializeEffect;
    private TextMeshPro messageTextTMP;


    private bool isEnabled = false;
    private ChestState chestState = ChestState.closed;
    private GameObject chestItemGameObject;
    private ChestItem chestItem;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponentInChildren<TextMeshPro>();
    }

    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponDetailsSO weaponDetails, int ammoPercent)
    {
        this.healthPercent = healthPercent;
        this.weaponDetails = weaponDetails;
        this.ammoPercent = ammoPercent;

        if (shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] {spriteRenderer};

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(
            GameResources.Instance.materializeShader,
            materializeColor,
            materializeTime,
            spriteRendererArray,
            GameResources.Instance.litMaterial));

        EnableChest();
    }

    private void EnableChest()
    {
        isEnabled = true;
    }

    public void UseItem()
    {
        if (!isEnabled) return;

        switch (chestState)
        {
            case ChestState.closed:
                OpenChest();
                break;

            case ChestState.healthItem:
                CollectHealthItem();
                break;

            case ChestState.ammoItem:
                CollectAmmoItem();
                break;

            case ChestState.weaponItem:
                CollectWeaponItem();
                break;

            case ChestState.empty:
                return;

            default:
                return;
        }
    }

    private void OpenChest()
    {
        animator.SetBool(Settings.use, true);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpenSoundEffect);

        if(weaponDetails != null)
        {
            if (GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
            {
                weaponDetails = null;
            }
        }

        UpdateChestState();
    }
    private void CollectHealthItem()
    {
        if(chestItem == null || !chestItem.isItemMaterialized) return;

        GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickupSoundEffect);

        healthPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    private void CollectAmmoItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer();

        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPercent);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickupSoundEffect);

        ammoPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    private void CollectWeaponItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickupSoundEffect);
        }
        else
        {
            StartCoroutine(DisplayMessage("WEAPON\nALREADY\nEQUIPPED", 5f));
        }

        weaponDetails = null;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    private void UpdateChestState()
    {
        if(healthPercent != 0)
        {
            chestState = ChestState.healthItem;
            InstantiateHealthItem();
        }
        else if(ammoPercent != 0)
        {
            chestState = ChestState.ammoItem;
            InstantiateAmmoItem();
        }
        else if(weaponDetails != null)
        {
            chestState = ChestState.weaponItem;
            InstantiateWeaponItem();
        }
        else
        {
            chestState = ChestState.empty;
        }
    }

    private void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }

    private void InstantiateHealthItem()
    {
        InstantiateItem();

        chestItem.Initialize(   GameResources.Instance.heartIcon,
                                healthPercent.ToString() + "%",
                                itemSpawnPoint.position,
                                materializeColor);
    }

    private void InstantiateAmmoItem()
    {
        InstantiateItem();

        chestItem.Initialize(   GameResources.Instance.bulletIcon,
                                ammoPercent.ToString() + "%",
                                itemSpawnPoint.position,
                                materializeColor);
    }

    private void InstantiateWeaponItem()
    {
        InstantiateItem();

        chestItem.Initialize(   weaponDetails.weaponSprite,
                                weaponDetails.weaponName,
                                itemSpawnPoint.position,
                                materializeColor);
    }

    private IEnumerator DisplayMessage(string message, float time)
    {
        messageTextTMP.text = message;

        yield return new WaitForSeconds(time);

        messageTextTMP.text = "";
    }
}
