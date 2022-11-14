using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent, ReloadWeaponEventArg> OnReloadWeapon;

    public void CallReloadWeaponEvent(Weapon weapon, int topUpAmmoPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArg()
        {
            weapon = weapon,
            topUpAmmoPercent = topUpAmmoPercent
        });
    }
}

public class ReloadWeaponEventArg : EventArgs
{
    public Weapon weapon;
    public int topUpAmmoPercent;
}
