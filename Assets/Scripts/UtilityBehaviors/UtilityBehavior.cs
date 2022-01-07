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
        var considerationScore = 0.00f;

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

            considerationScore = consideration.GetWeight(behaviorController.propertyList[consideration.name]);

            if (optOutConsiderations.Contains(consideration.name) && considerationScore == 0) {
                score = 0;
                return score;
            }

            score += considerationScore;
        }

        score /= considerations.Length;

        if (score < 0) {
            score = 0;
        }

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
