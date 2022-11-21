using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivateRooms : MonoBehaviour
{
    [Header("MINIMAP CAMERA")]
    [SerializeField] private Camera minimapCamera;

    private void Start()
    {
        InvokeRepeating(nameof(EnableRooms), 0.5f, 2f);
    }

    private void EnableRooms()
    {
        foreach(KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            HelperUtilities.CameraWorldPositionBounds(out Vector2Int minimapCameraWorldLowerBounds,
                out Vector2Int minimapCameraWorldUpperBounds, minimapCamera);

            if((room.lowerBounds.x <= minimapCameraWorldUpperBounds.x && room.lowerBounds.y <= minimapCameraWorldUpperBounds.y) &&
                (room.upperBounds.x >= minimapCameraWorldLowerBounds.x && room.upperBounds.y >= minimapCameraWorldLowerBounds.y))
            {
                room.instantiatedRoom.gameObject.SetActive(true);
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
