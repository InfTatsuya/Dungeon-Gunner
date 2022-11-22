using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivateRooms : MonoBehaviour
{
    [Header("MINIMAP CAMERA")]
    [SerializeField] private Camera minimapCamera;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        InvokeRepeating(nameof(EnableRooms), 0.5f, 2f);
    }

    private void EnableRooms()
    {
        HelperUtilities.CameraWorldPositionBounds(out Vector2Int minimapCameraWorldLowerBounds,
                out Vector2Int minimapCameraWorldUpperBounds, minimapCamera);

        HelperUtilities.CameraWorldPositionBounds(out Vector2Int mainCameraWorldLowerBounds,
                out Vector2Int mainCameraWorldUpperBounds, mainCamera);

        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if((room.lowerBounds.x <= minimapCameraWorldUpperBounds.x && room.lowerBounds.y <= minimapCameraWorldUpperBounds.y) &&
                (room.upperBounds.x >= minimapCameraWorldLowerBounds.x && room.upperBounds.y >= minimapCameraWorldLowerBounds.y))
            {
                room.instantiatedRoom.gameObject.SetActive(true);

                if ((room.lowerBounds.x <= mainCameraWorldUpperBounds.x && room.lowerBounds.y <= mainCameraWorldUpperBounds.y) &&
                (room.upperBounds.x >= mainCameraWorldLowerBounds.x && room.upperBounds.y >= mainCameraWorldLowerBounds.y))
                {
                    room.instantiatedRoom.ActivateEnviromentGameObject();
                }
                else
                {
                    room.instantiatedRoom.DeactivateEnviromentGameObject();
                }
            }
            else
            {
                room.instantiatedRoom.gameObject.SetActive(false);
            }
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(minimapCamera), minimapCamera);
    }
#endif
    #endregion
}
