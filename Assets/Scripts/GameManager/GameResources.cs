using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    #region Header DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
    #endregion

    public RoomNodeTypeListSO roomNodeTypeList;

    [Space(10)]
    [Header("PLAYER"), Tooltip("The current player SO - used to ref the current player betwwen scenes")]
    public CurrentPlayerSO currentPlayer;


    [Space(10)]
    [Header("MUSIC")]

    [Tooltip("The music master mixer group")]
    public AudioMixerGroup musicMasterMixerGroup;

    [Tooltip("music on full snapshot")]
    public AudioMixerSnapshot musicOnFullSnapshot;

    [Tooltip("music on low snapshot")]
    public AudioMixerSnapshot musicOnLowSnapshot;

    [Tooltip("music off snapshot")]
    public AudioMixerSnapshot musicOffSnapshot;


    [Space(10)]
    [Header("SOUNDS"), Tooltip("The sounds master mixer group")]
    public AudioMixerGroup soundMasterMixerGroup;

    [Tooltip("Door open close sound effect")]
    public SoundEffectSO doorOpenCloseSoundEffect;

    [Tooltip("Table flip sound effect")]
    public SoundEffectSO tableFlipSoundEffect;

    [Tooltip("Chest open sound effect")]
    public SoundEffectSO chestOpenSoundEffect;

    [Tooltip("Health pickup sound effect")]
    public SoundEffectSO healthPickupSoundEffect;

    [Tooltip("Weapon pickup sound effect")]
    public SoundEffectSO weaponPickupSoundEffect;

    [Tooltip("Ammo pickup sound effect")]
    public SoundEffectSO ammoPickupSoundEffect;



    [Space(10)]
    [Header("MATERIALS"), Tooltip("Dimmed Material")]
    public Material dimmedMaterial;

    [Tooltip("Sprite-Lit-Default Material")]
    public Material litMaterial;

    [Tooltip("The Variable Lit Shader")]
    public Shader variableLitShader;

    [Tooltip("materialize Shader")]
    public Shader materializeShader;



    [Space(10)]
    [Header("SPECIAL TILEMAP TILES")]

    [Tooltip("Collision tiles that the enemies cannot navigate to")]
    public TileBase[] enemyUnwalkableCollisionTileArray;

    [Tooltip("Preferred path tile for enemy navigation")]
    public TileBase preferredEnemyPathTile;



    [Space(10)]
    [Header("UI")]

    [Tooltip("The ammo icon prefab")]
    public GameObject ammoIconPrefab;

    [Tooltip("The heart UI prefab")]
    public GameObject heartPrefab;




    [Space(10)]
    [Header("CHESTS")]

    [Tooltip("Chest Item prefab")]
    public GameObject chestItemPrefab;

    [Tooltip("Heart icon sprite")]
    public Sprite heartIcon;

    [Tooltip("Bullet icon sprite")]
    public Sprite bulletIcon;




    [Space(10)]
    [Header("MINIMAP")]

    [Tooltip("Minimap Skull Prefab")]
    public GameObject minimapSkullPrefab;

    #region Validation

#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(roomNodeTypeList), roomNodeTypeList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(currentPlayer), currentPlayer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundMasterMixerGroup), soundMasterMixerGroup);
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorOpenCloseSoundEffect), doorOpenCloseSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(dimmedMaterial), dimmedMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(litMaterial), litMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(variableLitShader), variableLitShader);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyUnwalkableCollisionTileArray), enemyUnwalkableCollisionTileArray);
        HelperUtilities.ValidateCheckNullValue(this, nameof(preferredEnemyPathTile), preferredEnemyPathTile);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoIconPrefab), ammoIconPrefab);
        HelperUtilities.ValidateCheckNullValue(this, nameof(tableFlipSoundEffect), tableFlipSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(heartPrefab), heartPrefab);
        HelperUtilities.ValidateCheckNullValue(this, nameof(chestOpenSoundEffect), chestOpenSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(healthPickupSoundEffect), healthPickupSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponPickupSoundEffect), weaponPickupSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoPickupSoundEffect), ammoPickupSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(chestItemPrefab), chestItemPrefab);
        HelperUtilities.ValidateCheckNullValue(this, nameof(heartIcon), heartIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(bulletIcon), bulletIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(materializeShader), materializeShader);
        HelperUtilities.ValidateCheckNullValue(this, nameof(minimapSkullPrefab), minimapSkullPrefab);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicMasterMixerGroup), musicMasterMixerGroup);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicOnFullSnapshot), musicOnFullSnapshot);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicOnLowSnapshot), musicOnLowSnapshot);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicOffSnapshot), musicOffSnapshot);
    }
#endif
    #endregion
}
