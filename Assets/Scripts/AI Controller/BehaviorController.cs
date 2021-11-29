using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BehaviorController : MonoBehaviour
{
    public bool debug;
    public UtilityBehavior[] utilityBehaviors;
    public UtilityBehavior defaultUtilityBehavior;
    public List<float> utilWeights = new List<float>();
    public float currentUtilityBehaviorWeight;
    public UtilityBehavior currentUtilityBehavior;
    public event EventHandler OnContinuePreviousPath;
    public float behaviorUpdateTime;
    private float startBehaviorUpdateTime;

    private Text currentBehaviorText;
    private Text allBehaviorsText;

    private class OptOutParameters {
        public bool playerIsVisible;
        public bool playerLastPositionIsKnown;
        public bool canAttack;
    }

    private class OptInParameters {
        public bool playerIsVisible;
        public bool playerLastPositionIsKnown;
        public bool canAttack;
    }

    void Start()
    {
        if (debug) {
            currentBehaviorText = GameObject.FindGameObjectWithTag("CurrentBehavior").GetComponent<Text>();
            allBehaviorsText = GameObject.FindGameObjectWithTag("AllBehaviors").GetComponent<Text>();
        }

        startBehaviorUpdateTime = behaviorUpdateTime;

        if (utilityBehaviors.Length > 0) {
            foreach (var behavior in utilityBehaviors) {
                behavior.UpdateBehavior(this);
                utilWeights.Add(behavior.weight);
            }

            currentUtilityBehavior = GetHighestUtility();
            currentUtilityBehavior.Trigger(this);
            currentBehaviorText.text = currentUtilityBehavior.name;
        }
    }

    void Update()
    {
        if (behaviorUpdateTime >= 0) {
            behaviorUpdateTime -= Time.deltaTime;
        } else {
            utilWeights.Clear();
            allBehaviorsText.text = "";
            foreach (var behavior in utilityBehaviors)
            {
                utilWeights.Add(behavior.UpdateBehavior(this));
                allBehaviorsText.text += behavior.name + ": " + behavior.weight + "\n";
            }

            if (currentUtilityBehavior != null && currentUtilityBehavior != GetHighestUtility()) {
                currentUtilityBehavior.Reset(this);
                currentUtilityBehavior = GetHighestUtility();
                currentUtilityBehavior.Trigger(this);
                currentBehaviorText.text = currentUtilityBehavior.name;
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

            return utilityBehaviors.Where(ub => ub.rank == highestRank).First();
        }

        return utilityBehaviors[utilWeights.IndexOf(highestWeight)];
    }
}
