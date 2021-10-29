using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UtilityBehavior : MonoBehaviour
{
    public float weight;
    public bool isActive;

    public abstract void UpdateBehavior(BehaviorController behaviorController);

    public abstract void Trigger(BehaviorController behaviorController);

    public abstract void Reset(BehaviorController behaviorController);
}
