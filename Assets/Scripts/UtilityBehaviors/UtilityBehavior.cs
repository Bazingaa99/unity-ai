using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

abstract public class UtilityBehavior : MonoBehaviour, ISerializationCallbackReceiver
{
    public float score;
    public bool isActive;
    public int rank;
    public float cooldown;

    public Consideration[] considerations;

    public string[] reverseConsiderations;
    public string[] optOutConsiderations;

    public float UpdateBehavior(BehaviorController behaviorController)
    {
        score = 0;
        var considerationWeight = 0.00f;

        if (cooldown > 0) {
            cooldown -= behaviorController.startBehaviorUpdateTime;
            score = 0;
            return score;
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

            considerationWeight = consideration.GetWeight(behaviorController.propertyList[consideration.name]);

            if (optOutConsiderations.Contains(consideration.name) && considerationWeight == 0) {
                score = 0;
                return score;
            }

            score += considerationWeight;
        }

        // var modificationFactor = 1.00f - (1.00f / considerations.Length);
        // var makeUpValue = (1 - weight) * modificationFactor;
        // weight += makeUpValue * weight;
        score /= considerations.Length;

        return score;
    }

    public abstract void Trigger(BehaviorController behaviorController);

    public abstract void Reset(BehaviorController behaviorController);

    public void OnBeforeSerialize() 
    {
        score = 0;
        isActive = false;
        cooldown = 0;
    }

    public void OnAfterDeserialize()
    {

    }
}
