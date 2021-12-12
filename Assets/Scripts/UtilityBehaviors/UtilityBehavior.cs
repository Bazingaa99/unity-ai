using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

abstract public class UtilityBehavior : MonoBehaviour, ISerializationCallbackReceiver
{
    public float weight;
    public bool isActive;
    public int rank;
    public float cooldown;

    public Consideration[] considerations;

    public string[] reverseConsiderations;
    public string[] optOutConsiderations;

    public float UpdateBehavior(BehaviorController behaviorController)
    {
        weight = 0;
        var considerationWeight = 0.00f;

        if (cooldown > 0) {
            cooldown -= behaviorController.startBehaviorUpdateTime;
            weight = 0;
            return weight;
        }

        foreach (Consideration consideration in considerations) {
            if (!consideration.isEnabled) {
                continue;
            }

            if (reverseConsiderations.Contains(consideration.name)) {
                consideration.reverse = true;
            } else {
                consideration.reverse = false;
            }

            considerationWeight = consideration.getWeight(behaviorController.considerationProperties.propertyList[consideration.name]);

            if (optOutConsiderations.Contains(consideration.name) && considerationWeight == 0) {
                weight = 0;
                return weight;
            }

            weight += considerationWeight;
        }

        // var modificationFactor = 1.00f - (1.00f / considerations.Length);
        // var makeUpValue = (1 - weight) * modificationFactor;
        // weight += makeUpValue * weight;
        weight /= considerations.Length;

        return weight;
    }

    public abstract void Trigger(BehaviorController behaviorController);

    public abstract void Reset(BehaviorController behaviorController);

    public void OnBeforeSerialize() 
    {
        weight = 0;
        isActive = false;
        cooldown = 0;
    }

    public void OnAfterDeserialize()
    {

    }
}
