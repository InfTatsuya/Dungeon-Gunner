using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="DungeonLevel_", menuName = "SO/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    [Space(10)]
    [Header("BASIC LEVEL DETAILS"), Tooltip("The name for the level")]
    public string levelName;

    [Space(10)]
    [Header("ROOM TEMPLATES FOR LEVEL")]
    public List<RoomTemplateSO> roomTemplateList;

    [Space(10)]
    [Header("ROOM NODE GRAPHS FOR LEVEL")]
    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation

#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(levelName), levelName);

        if(HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
        {
            return;
        }
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
        {
            return;
        }

        //Check that room temnplates are specified for all the node types in the node graphs
        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;

        foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;

            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isEWCorridor = true;
            if (roomTemplateSO.roomNodeType.isCorridorNS) 
                isNSCorridor = true;

            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;
        }

        if (!isEWCorridor)
        {
            Debug.Log("In " + this.name.ToString() + " no EW Corridor");
        }
        if (!isNSCorridor)
        {
            Debug.Log("In " + this.name.ToString() + " no NS Corridor");
        }
        if (!isEntrance)
        {
            Debug.Log("In " + this.name.ToString() + " no Entrance");
        }

        foreach(RoomNodeGraphSO roomNodeGraphSO in roomNodeGraphList)
        {
            if (roomNodeGraphSO == null) return;

            foreach(RoomNodeSO roomNodeSO in roomNodeGraphSO.roomNodeList)
            {
                if(roomNodeSO == null) continue;

                if (roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW ||
                   roomNodeSO.roomNodeType.isCorridorNS || roomNodeSO.roomNodeType.isCorridor ||
                   roomNodeSO.roomNodeType.isNone) continue;

                bool isRoomNodeTypeFound = false;

                foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if(roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }

                if (!isRoomNodeTypeFound)
                {
                    Debug.Log("In " + this.name.ToString() +": No Room Template found in" + roomNodeGraphSO.name.ToString());
                }
            }
        }
    }

#endif

    #endregion
}
