using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    [Tooltip("MovementDetailsSO for define movement details such as speed, ...")]
    [SerializeField] private MovementDetailsSO movementDetails;

    [HideInInspector] public float moveSpeed;

    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>();
    private Vector3 playerRefPosition;
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown;
    private WaitForFixedUpdate waitForFixedUpdate;
    private bool chasePlayer = false;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        playerRefPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        if( !chasePlayer && 
            Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) 
                        < enemy.enemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }

        if (!chasePlayer) return;

        if( currentEnemyPathRebuildCooldown <= 0f ||
            Vector3.Distance(playerRefPosition, GameManager.Instance.GetPlayer().GetPlayerPosition()) 
                > Settings.playerMoveDistanceToRebuildPath)
        {
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;

            playerRefPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            CreatePath();

            if(movementSteps != null)
            {
                if(moveEnemyRoutine != null)
                {
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }

                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }
        }
    }

    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while(movementSteps.Count > 0)
        {
            Vector3 nextPos = movementSteps.Pop();

            while(Vector3.Distance(nextPos, transform.position) > 0.2f)
            {
                enemy.movementToPositionEvent.CallMovementToPositionEvent(
                    nextPos,
                    transform.position,
                    moveSpeed,
                    (nextPos - transform.position).normalized);

                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate;
        }

        enemy.idleEvent.CallIdleEvent();
    }

    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3Int enemyGridPos = grid.WorldToCell(transform.position);

        Vector3Int playerGridPos = GetNearestNonObstaclePlayerPosition(currentRoom);

        movementSteps = AStar.BuildPath(currentRoom, enemyGridPos, playerGridPos);

        if (movementSteps != null)
        {
            movementSteps.Pop();
        }
        else
        {
            enemy.idleEvent.CallIdleEvent();
        }
    }

    private Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom)
    {
        Vector3 playerPos = GameManager.Instance.GetPlayer().GetPlayerPosition();

        Vector3Int playerCellPos = currentRoom.instantiatedRoom.grid.WorldToCell(playerPos);

        Vector2Int adjustedPlayerCellPos = new Vector2Int(
            playerCellPos.x - currentRoom.templateLowerBounds.x,
            playerCellPos.y - currentRoom.templateLowerBounds.y);

        int obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPos.x, adjustedPlayerCellPos.y];

        if(obstacle != 0)
        {
            return playerCellPos;
        }
        else
        {
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    try
                    {
                        obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty
                            [adjustedPlayerCellPos.x + i, adjustedPlayerCellPos.y + j];

                        if(obstacle != 0) 
                            return new Vector3Int(playerCellPos.x + i, playerCellPos.y + j);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return playerCellPos;
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion
}
