using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class DungeonMap : SingletonMonoBehaviour<DungeonMap>
{
    [Space(10)]
    [Header("GAMEOBJECTS REFERENCES")]

    [Tooltip("The MinimapUI gameobject")]
    [SerializeField] private GameObject minimapUI;

    private Camera dungeonMapCamera;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        Transform playerTransform = GameManager.Instance.GetPlayer().transform;

        CinemachineVirtualCamera cmVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cmVirtualCamera.Follow = playerTransform;

        dungeonMapCamera = GetComponentInChildren<Camera>();
        dungeonMapCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && GameManager.Instance.gameState == GameState.dungeonOverviewMap)
        {
            GetRoomClicked();
        }
    }

    private void GetRoomClicked()
    {
        Vector3 worldPos = dungeonMapCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(new Vector2(worldPos.x, worldPos.y), 1f);

        foreach(Collider2D collider2D in collider2DArray)
        {
            if(collider2D.GetComponent<InstantiatedRoom>() != null)
            {
                InstantiatedRoom instantiatedRoom = collider2D.GetComponent<InstantiatedRoom>();

                if(instantiatedRoom.room.isClearedOfEnemies && instantiatedRoom.room.isPreviouslyVisited)
                {
                    StartCoroutine(MovePlayerToRoom(worldPos, instantiatedRoom.room));
                }
            }
        }
    }

    private IEnumerator MovePlayerToRoom(Vector3 worldPos, Room room)
    {
        StaticEventHandler.CallRoomChangedEvent(room);

        yield return StartCoroutine(GameManager.Instance.Fade(0f, 1f, 0f, Color.black));

        ClearDungeonOverviewMap();

        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        Vector3 spawnPos = HelperUtilities.GetSpawnPositionNearestToPlayer(worldPos);

        GameManager.Instance.GetPlayer().transform.position = spawnPos;

        yield return StartCoroutine(GameManager.Instance.Fade(1f, 0f, 1f, Color.black));

        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();
    }

    public void DisplayDungeonOverViewMap()
    {
        GameManager.Instance.previousGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        mainCamera.gameObject.SetActive(false);
        dungeonMapCamera.gameObject.SetActive(true);

        ActiveRoomsForDisplay();

        minimapUI.SetActive(false);
    }

    public void ClearDungeonOverviewMap()
    {
        GameManager.Instance.gameState = GameManager.Instance.previousGameState;
        GameManager.Instance.previousGameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();

        mainCamera.gameObject.SetActive(true);
        dungeonMapCamera.gameObject.SetActive(false);

        minimapUI.SetActive(true);
    }

    private void ActiveRoomsForDisplay()
    {
        foreach(KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            room.instantiatedRoom.gameObject.SetActive(true);
        }
    }
}
