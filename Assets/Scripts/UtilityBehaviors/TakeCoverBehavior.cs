using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoverBehavior : UtilityBehavior
{
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        ItemHandler itemHandler = behaviorController.GetComponent<ItemHandler>();
        RangedWeapon rangedWeapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        weight = Mathf.Abs(rangedWeapon.ammo - rangedWeapon.maxAmmo) / rangedWeapon.maxAmmo;
        

        return weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.takeCover = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.takeCover = false;
    }
}
