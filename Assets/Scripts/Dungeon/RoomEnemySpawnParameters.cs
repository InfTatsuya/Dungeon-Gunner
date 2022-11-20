using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters 
{
    [Tooltip("Defines the dungeon level for this room with regard to how many enemies should be spawned")]
    public DungeonLevelSO dungeonLevel;

    [Tooltip("The minimum number of enemies")]
    public int minTotalEnemiesToSpawn;

    [Tooltip("The maximum number of enemies")]
    public int maxTotalEnemiesToSpawn;

    [Tooltip("The minimum number of concurrent enemies")]
    public int minConcurrentEnemies;

    [Tooltip("The maximum number of concurrent enemies")]
    public int maxConcurrentEnemies;

    [Tooltip("The minimum spawn interval in seconds")]
    public int minSpawnInterval;

    [Tooltip("The maximum spawn interval in seconds")]
    public int maxSpawnInterval;
}
