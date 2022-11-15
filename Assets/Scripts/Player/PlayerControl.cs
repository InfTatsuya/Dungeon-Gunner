using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    [SerializeField, Tooltip("MovementDetailsSO contains details such as speed,....")]
    private MovementDetailsSO movementDetails;

    //[SerializeField, Tooltip("The player WeaponShootPosition gameobject in the hieracrchy")]
    //private Transform weaponShootPosition;

    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private bool isPlayerRolling = false;
    private float playerRollCooldownTimer = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        SetStartingWeapon();

        SetPlayerAnimationSpeed();
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach(Weapon weapon in player.weaponList)
        {
            if(weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }

            index++;
        }
    }

    private void SetPlayerAnimationSpeed()
    {
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimations;
    }

    private void Update()
    {
        if (isPlayerRolling) return;

        MovementInput();

        WeaponInput();

        PlayerRollCooldownTimer();
    }

    private void MovementInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButton(1);

        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        if(horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        if(direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            else if(playerRollCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }
        }
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    private void PlayerRoll(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        float minDistance = 0.2f;

        isPlayerRolling = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;

        while(Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed, direction, isPlayerRolling);

            yield return waitForFixedUpdate;
        }

        isPlayerRolling = false;

        playerRollCooldownTimer = movementDetails.rollCooldownTime;

        player.transform.position = targetPosition;
    }

    private void PlayerRollCooldownTimer()
    {
        if(playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDir;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDir);

        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDir);

        ReloadWeaponInput();
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDir)
    {
        Vector3 mouseWorldPos = HelperUtilities.GetMouseWorldPosition();

        weaponDirection = mouseWorldPos - player.activeWeapon.GetShootPosition();

        Vector3 playerDir = mouseWorldPos - transform.position;

        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDir);
        playerAimDir = HelperUtilities.GetAimDirection(playerAngleDegrees);

        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDir, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDir)
    {
        if (Input.GetMouseButton(0))
        {
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDir, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
            leftMouseDownPreviousFrame = true;
        }
        else
        {
            leftMouseDownPreviousFrame = false;
        }
    }

    private void SetWeaponByIndex(int index)
    {
        if(index - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = index;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[index - 1]);
        }
    }

    private void ReloadWeaponInput()
    {
        Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

        if (currentWeapon.isWeaponReloading) return;

        if (currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponClipAmmoCapacity &&
            !currentWeapon.weaponDetails.hasInfiniteAmmo) return;

        if (currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponDetails.weaponClipAmmoCapacity)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopPlayerRollCoroutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StopPlayerRollCoroutine();
    }

    private void StopPlayerRollCoroutine()
    {
        if(playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);

            isPlayerRolling = false;
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
