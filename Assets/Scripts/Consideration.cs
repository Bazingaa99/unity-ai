using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Consideration : MonoBehaviour
{
    public float property;
    public bool reverse;
    public bool isEnabled = true;
    public float score;

    public AnimationCurve utilityCurve;

    public float GetWeight(float property) {
        this.property = property;
        this.score = utilityCurve.Evaluate(property);

        if (reverse) {
            this.score = utilityCurve.Evaluate(1 - property);
            return this.score;
        } else {
            this.score = utilityCurve.Evaluate(property);
            return this.score;
        }
    }
}
