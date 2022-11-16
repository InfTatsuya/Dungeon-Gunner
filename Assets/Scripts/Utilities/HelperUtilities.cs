using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{
    public static Camera mainCamera;

    public static Vector3 GetMouseWorldPosition()
    {
        if(mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.x = Mathf.Clamp(mouseScreenPos.x, 0f, Screen.width);
        mouseScreenPos.y = Mathf.Clamp(mouseScreenPos.y, 0f, Screen.height);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        return mouseWorldPos;
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);

        return radians * Mathf.Rad2Deg;
    }

    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad) ,0f);

        return directionVector;
    }

    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDir = AimDirection.Left;

        if(angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDir = AimDirection.UpRight;
        }
        else if(angleDegrees > 67f && angleDegrees <= 112f)
        {
            aimDir = AimDirection.Up;
        }
        else if(angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDir = AimDirection.UpLeft;
        }
        else if((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180f && angleDegrees <= -135f))
        {
            aimDir = AimDirection.Left;
        }
        else if(angleDegrees > -135f && angleDegrees <= -45f)
        {
            aimDir = AimDirection.Down;
        }
        else if((angleDegrees <= 0f && angleDegrees > -45f) || (angleDegrees > 0 && angleDegrees <= 22f))
        {
            aimDir = AimDirection.Right;
        }

        return aimDir;
    }

    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;

        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }

    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object thisObject, string fileName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fileName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
    {
        if(objectToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " +thisObject.name.ToString());
            return true;
        }

        return false;
    }

    /// <summary>
    /// list empty or contains null value check - returns true if there is an error
    /// </summary>
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fileName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if(enumerableObjectToCheck == null)
        {
            Debug.Log(fileName + " is null in " + thisObject.name.ToString());
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {

            if (item == null)
            {
                Debug.Log(fileName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fileName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }

    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if(valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }

        return error;
    }

    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }

        return error;
    }

    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimun, float valueToCheckMinimun, string fieldNameMaximun, float valueToCheckMaximun, bool isZeroAllowed)
    {
        bool error = false;
        if(valueToCheckMinimun > valueToCheckMaximun)
        {
            Debug.Log(fieldNameMinimun + " must be less than " + fieldNameMaximun + " in object " + thisObject.name.ToString());
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimun, valueToCheckMinimun, isZeroAllowed)) error = true;

        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximun, valueToCheckMaximun, isZeroAllowed)) error = true;

        return error;
    }

    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPos)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3 nearestPosition = new Vector3(1000f, 1000f, 0f);

        foreach(Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            Vector3 spawnPosWorld = grid.CellToWorld((Vector3Int) spawnPositionGrid);

            if(Vector3.Distance(spawnPosWorld, playerPos) < Vector3.Distance(nearestPosition, playerPos))
            {
                nearestPosition = spawnPosWorld;
            }
        }

        return nearestPosition;
    }
}
