using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Consideration : MonoBehaviour, ISerializationCallbackReceiver
{
    public string considerationName;
    public float property;
    public bool reverse;
    public bool enabled = true;
    public float weight;
    public float maxValue;

    public AnimationCurve utilityCurve;

    public float getWeight(float property) {
        this.property = property;
        this.weight = utilityCurve.Evaluate(property);

        if (reverse) {
            return utilityCurve.Evaluate(1 - property);
        } else {
            return utilityCurve.Evaluate(property);
        }
        
    }

    public void OnBeforeSerialize() 
    {
        // utilityCurve.
    }

    public void OnAfterDeserialize()
    {

    }
}
