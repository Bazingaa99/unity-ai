using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class BehaviorController : MonoBehaviour
{
    public UtilityBehavior[] utilityBehaviors;
    public UtilityBehavior defaultUtilityBehavior;
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

        if (utilityBehaviors.Length > 0) {
            foreach (var behavior in utilityBehaviors) {
                behavior.UpdateBehavior(this);
                utilWeights.Add(behavior.weight);
            }

            currentUtilityBehavior = GetHighestUtility();
            currentUtilityBehavior.Trigger(this);
        }
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



            if (currentUtilityBehavior != null && currentUtilityBehavior != GetHighestUtility()) {
                currentUtilityBehavior.Reset(this);
                currentUtilityBehavior = GetHighestUtility();
                currentUtilityBehavior.Trigger(this);
            }

            behaviorUpdateTime = startBehaviorUpdateTime;
        }
    }

    private UtilityBehavior GetHighestUtility()
    {
        float highestWeight = utilWeights.Max();
        if (highestWeight == 0) {
            return defaultUtilityBehavior;
        }

        int[] indices = utilWeights.Select((x, i) => new { Index = i, Value = x }).Where(x => x.Value == highestWeight).Select(x => x.Index).ToArray();
        
        if (indices.Length > 1) {
            List<int> ranks = new List<int>();
            foreach (int index in indices)
            {
                ranks.Add(utilityBehaviors[index].rank);
            }
            int highestRank = ranks.Max();

            return utilityBehaviors[utilWeights.IndexOf(highestRank)];
        }

        return utilityBehaviors[utilWeights.IndexOf(highestWeight)];
    }
}
