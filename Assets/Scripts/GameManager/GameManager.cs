using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Space(10)]
    [Header("DUNGEON LEVELS"), Tooltip("Populate with the dungeon level SO")]
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    [HideInInspector] public GameState gameState;

    void Start()
    {
        gameState = GameState.gameStarted;
    }

    void Update()
    {
        HandleGameState();

        if (Input.GetKeyUp(KeyCode.R))
        {
            gameState = GameState.gameStarted;
        }
    }

    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.gameStarted:
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;
                break;


        }
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        bool dungeonBuildSuccessful = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuildSuccessful)
        {
            Debug.Log("Couldn't build dungeon!!!!");
        }
    }

    #region Validation  
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);

    }

#endif

    #endregion
}
