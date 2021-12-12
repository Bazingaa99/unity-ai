using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBehavior : UtilityBehavior
{
    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        cooldown = combatController.itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>().maxReloadTime;
        combatController.Reload();
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
    }
}
