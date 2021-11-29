using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBehavior : UtilityBehavior
{
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        ItemHandler itemHandler = behaviorController.GetComponent<ItemHandler>();
        RangedWeapon rangedWeapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        weight = Mathf.Abs(rangedWeapon.ammo - rangedWeapon.maxAmmo) / rangedWeapon.maxAmmo;

        return weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        ItemHandler itemHandler = behaviorController.GetComponent<ItemHandler>();
        RangedWeapon rangedWeapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        rangedWeapon.Reload();
        navigationController.TakeCover(sensorController.objectTransform);
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
    }
}
