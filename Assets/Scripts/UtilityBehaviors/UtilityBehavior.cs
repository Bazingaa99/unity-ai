using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UtilityBehavior : MonoBehaviour, ISerializationCallbackReceiver
{
    public float weight;
    public bool isActive;
    public int rank;
    public bool playerRequired;

    public abstract float UpdateBehavior(BehaviorController behaviorController);

    public abstract void Trigger(BehaviorController behaviorController);

    public abstract void Reset(BehaviorController behaviorController);

    public void OnBeforeSerialize() 
    {
        weight = 0;
        isActive = false;
    }

    public void OnAfterDeserialize()
    {

    }
}
