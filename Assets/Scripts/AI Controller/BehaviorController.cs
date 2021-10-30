using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class BehaviorController : MonoBehaviour
{
    public UtilityBehavior[] utilityBehaviors;
    public List<float> utilWeights = new List<float>();
    public float currentUtilityBehaviorWeight;
    public UtilityBehavior currentUtilityBehavior;
    public event EventHandler OnContinuePreviousPath;

    public float behaviorUpdateTime;
    private float startBehaviorUpdateTime;

    // Start is called before the first frame update
    void Start()
    {
        startBehaviorUpdateTime = behaviorUpdateTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (behaviorUpdateTime >= 0) {
            behaviorUpdateTime -= Time.deltaTime;
        } else {
            utilWeights.Clear();
            foreach (var behavior in utilityBehaviors)
            {
                behavior.UpdateBehavior(this);
                utilWeights.Add(behavior.weight);
            }

            currentUtilityBehaviorWeight = utilWeights.Max();
            if (currentUtilityBehavior != null && currentUtilityBehavior != utilityBehaviors[utilWeights.IndexOf(currentUtilityBehaviorWeight)]) {
                currentUtilityBehavior.Reset(this);
                currentUtilityBehavior = utilityBehaviors[utilWeights.IndexOf(currentUtilityBehaviorWeight)];
                currentUtilityBehavior.Trigger(this);
            } else {
                currentUtilityBehavior = utilityBehaviors[utilWeights.IndexOf(currentUtilityBehaviorWeight)];
                currentUtilityBehavior.Trigger(this);
            }

            behaviorUpdateTime = startBehaviorUpdateTime;
        }
    }
}
