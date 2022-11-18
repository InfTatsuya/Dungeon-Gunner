using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        enemy.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        enemy.idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable()
    {
        enemy.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        enemy.idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        if(enemy.transform.position.x < GameManager.Instance.GetPlayer().transform.position.x)
        {
            SetAimWeaponAnimationParameters(AimDirection.Right);
        }
        else
        {
            SetAimWeaponAnimationParameters(AimDirection.Left);
        }

        SetMovementAnimationParameters();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    private void InitialiseAimAnimationParameters()
    {
        enemy.animator.SetBool(Settings.aimUp, false);
        enemy.animator.SetBool(Settings.aimDown, false);
        enemy.animator.SetBool(Settings.aimLeft, false);
        enemy.animator.SetBool(Settings.aimRight, false);
        enemy.animator.SetBool(Settings.aimUpLeft, false);
        enemy.animator.SetBool(Settings.aimUpRight, false);
    }

    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        InitialiseAimAnimationParameters();

        switch (aimDirection)
        {
            case AimDirection.Left:
                enemy.animator.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Right:
                enemy.animator.SetBool(Settings.aimRight, true);
                break;

            case AimDirection.Up:
                enemy.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.Down:
                enemy.animator.SetBool(Settings.aimDown, true);
                break;

            case AimDirection.UpLeft:
                enemy.animator.SetBool(Settings.aimUpLeft, true);
                break;

            case AimDirection.UpRight:
                enemy.animator.SetBool(Settings.aimUpRight, true);
                break;

            default:
                break;
        }
    }

    private void SetMovementAnimationParameters()
    {
        enemy.animator.SetBool(Settings.isIdle, false);
        enemy.animator.SetBool(Settings.isMoving, true);
    }

    private void SetIdleAnimationParameters()
    {
        enemy.animator.SetBool(Settings.isIdle, true);
        enemy.animator.SetBool(Settings.isMoving, false);
    }
}
