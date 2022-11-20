using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        enemiesSpawnedSoFar = 0;
        currentEnemyCount = 0;

        currentRoom = roomChangedEventArgs.room;

        if(currentRoom.roomNodeType.isCorridorEW || currentRoom.roomNodeType.isCorridorNS ||
            currentRoom.roomNodeType.isEntrance)
        {
            return;
        }

        if (currentRoom.isClearedOfEnemies) return;

        enemiesToSpawn = currentRoom.GetNumberOfEnemiesToSpawn(GameManager.Instance.GetCurrentDungeonLevel());
        roomEnemySpawnParameters = currentRoom.GetRoomEnemySpawnParameters(GameManager.Instance.GetCurrentDungeonLevel());

        if(enemiesToSpawn == 0)
        {
            currentRoom.isClearedOfEnemies = true;

            return;
        }

        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();

        currentRoom.instantiatedRoom.LockDoors();

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if(GameManager.Instance.gameState == GameState.playingLevel)
        {
            GameManager.Instance.previousGameState = GameState.playingLevel;
            GameManager.Instance.gameState = GameState.engagingEnemies;
        }

        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(currentRoom.enemiesByLevelList);

        if(currentRoom.spawnPositionArray.Length > 0)
        {
            for(int i = 0; i < enemiesToSpawn; i++)
            {
                while(currentEnemyCount >= enemyMaxConcurrentSpawnNumber)
                {
                    yield return null;
                }

                Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[
                    UnityEngine.Random.Range(0, currentRoom.spawnPositionArray.Length)];

                CreateEnemy(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPos));

                yield return new WaitForSeconds(GetEnemySpawnInterval());
            }
        }
    }

    private void CreateEnemy(EnemyDetailsSO enemyDetails, Vector3 position)
    {
        enemiesSpawnedSoFar++;
        currentEnemyCount++;

        DungeonLevelSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, position, Quaternion.identity, transform);

        enemy.GetComponent<Enemy>().EnemyInitialization(enemyDetails, enemiesSpawnedSoFar, dungeonLevel);
    }

    private float GetEnemySpawnInterval()
    {
        return UnityEngine.Random.Range(roomEnemySpawnParameters.minSpawnInterval, 
            roomEnemySpawnParameters.maxSpawnInterval);
    }

    private int GetConcurrentEnemies()
    {
        return UnityEngine.Random.Range(roomEnemySpawnParameters.minConcurrentEnemies,
            roomEnemySpawnParameters.maxConcurrentEnemies);
    }

}
