using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(InstantiatedRoom))]
[DisallowMultipleComponent]
public class RoomLightingControl : MonoBehaviour
{
    private InstantiatedRoom instantiatedRoom;

    private void Awake()
    {
        instantiatedRoom = GetComponent<InstantiatedRoom>();
    }

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
        if(roomChangedEventArgs.room == instantiatedRoom.room && !instantiatedRoom.room.isLit)
        {
            FadeInRoomLighting();

            instantiatedRoom.ActivateEnviromentGameObject();

            FadeInEnviromentLighting();

            FadeInDoors();

            instantiatedRoom.room.isLit = true;
        }
    }

    private void FadeInRoomLighting()
    {
        StartCoroutine(FadeInRoomLightingRoutine(instantiatedRoom));
    }

    private IEnumerator FadeInRoomLightingRoutine(InstantiatedRoom instantiatedRoom)
    {
        Material material = new Material(GameResources.Instance.variableLitShader);

        instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = material;

        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
    }

    private void FadeInEnviromentLighting()
    {
        Material material = new Material(GameResources.Instance.variableLitShader);

        Enviroment[] enviromentComponents = GetComponentsInChildren<Enviroment>();
        foreach(Enviroment enviroment in enviromentComponents)
        {
            if(enviroment.spriteRenderer != null)
            {
                enviroment.spriteRenderer.material = material;  
            }
        }

        StartCoroutine(FadeInEnviromentLightingRoutine(material, enviromentComponents));
    }

    private IEnumerator FadeInEnviromentLightingRoutine(Material material, Enviroment[] enviromentComponents)
    {
        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        foreach (Enviroment enviroment in enviromentComponents)
        {
            if (enviroment.spriteRenderer != null)
            {
                enviroment.spriteRenderer.material = GameResources.Instance.litMaterial;
            }
        }
    }

    private void FadeInDoors()
    {
        Door[] doorArray = GetComponentsInChildren<Door>();

        foreach(Door door in doorArray)
        {
            DoorLightingControl doorLightingControl = GetComponentInChildren<DoorLightingControl>();

            doorLightingControl.FadeInDoor(door);
        }
    }
}
