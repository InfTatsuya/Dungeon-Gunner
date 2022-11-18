using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarTest : MonoBehaviour
{
    private InstantiatedRoom instantiatedRoom;
    private Grid grid;
    private Tilemap frontTilemap;
    private Tilemap pathTilemap;
    private Vector3Int startPos;
    private Vector3Int endPos;
    private TileBase startPathTile;
    private TileBase finishPathTile;

    private Vector3Int noValue = new Vector3Int(999, 999, 999);
    private Stack<Vector3> pathStack;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void Start()
    {
        startPathTile = GameResources.Instance.preferredEnemyPathTile;
        finishPathTile = GameResources.Instance.enemyUnwalkableCollisionTileArray[0];
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        pathStack = null;
        instantiatedRoom = roomChangedEventArgs.room.instantiatedRoom;
        frontTilemap = instantiatedRoom.transform.Find("Grid/Tilemap4_Front").GetComponent<Tilemap>();
        grid = instantiatedRoom.transform.GetComponentInChildren<Grid>();
        startPos = noValue;
        endPos = noValue;

        SetUpPathTilemap();
    }

    private void SetUpPathTilemap()
    {
        Transform tilemapCloneTransform = instantiatedRoom.transform.Find("Grid/Tilemap4_Front(Clone)");

        if(tilemapCloneTransform == null)
        {
            pathTilemap = Instantiate(frontTilemap, grid.transform);
            pathTilemap.GetComponent<TilemapRenderer>().sortingOrder = 2;
            pathTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
            pathTilemap.gameObject.tag = "Untagged";
        }
        else
        {
            pathTilemap = instantiatedRoom.transform.Find("Grid/Tilemap4_Front(Clone)").GetComponent<Tilemap>();
            pathTilemap.ClearAllTiles();
        }
    }

    private void Update()
    {
        if (instantiatedRoom == null || startPathTile == null || finishPathTile == null ||
            grid == null || pathTilemap == null) return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            ClearPath();
            SetStartPosition();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ClearPath();
            SetEndPosition();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DisplayPath();
        }
    }

    private void SetStartPosition()
    {
        if(startPos == noValue)
        {
            startPos = grid.WorldToCell(HelperUtilities.GetMouseWorldPosition());

            if (!IsPositionWithinBounds(startPos))
            {
                startPos = noValue;
                return;
            }

            pathTilemap.SetTile(startPos, startPathTile);
        }
        else
        {
            pathTilemap.SetTile(startPos, null);
            startPos = noValue;
        }
    }
    private void SetEndPosition()
    {
        if (endPos == noValue)
        {
            endPos = grid.WorldToCell(HelperUtilities.GetMouseWorldPosition());

            if (!IsPositionWithinBounds(endPos))
            {
                endPos = noValue;
                return;
            }

            pathTilemap.SetTile(endPos, finishPathTile);
        }
        else
        {
            pathTilemap.SetTile(endPos, null);
            endPos = noValue;
        }
    }

    private bool IsPositionWithinBounds(Vector3Int position)
    {
        if( position.x < instantiatedRoom.room.templateLowerBounds.x ||
            position.x > instantiatedRoom.room.templateUpperBounds.x ||
            position.y < instantiatedRoom.room.templateLowerBounds.y ||
            position.y > instantiatedRoom.room.templateUpperBounds.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ClearPath()
    {
        if (pathStack == null) return;

        foreach(Vector3 worldPos in pathStack)
        {
            pathTilemap.SetTile(grid.WorldToCell(worldPos), null);
        }

        pathStack = null;

        endPos = noValue;
        startPos = noValue;
    }

    private void DisplayPath()
    {
        if (startPos == noValue || endPos == noValue) return;

        pathStack = AStar.BuildPath(instantiatedRoom.room, startPos, endPos);

        if (pathStack == null) return;

        foreach (Vector3 worldPos in pathStack)
        {
            pathTilemap.SetTile(grid.WorldToCell(worldPos), startPathTile);
        }
    }
}
