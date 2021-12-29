using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BehaviorController : MonoBehaviour
{
    public bool debug;
    public GameObject[] utilityBehaviors;
    public UtilityBehavior defaultUtilityBehavior;
    public List<float> utilScores = new List<float>();
    public float currentUtilityBehaviorWeight;
    public UtilityBehavior currentUtilityBehavior;
    private float behaviorUpdateTime = 0.1f;

    [HideInInspector]
    public float startBehaviorUpdateTime;
    private Text currentBehaviorText;
    private Text allBehaviorsText;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();

    void Start()
    {
        if (debug) {
            currentBehaviorText = GameObject.FindGameObjectWithTag("CurrentBehavior").GetComponent<Text>();
            allBehaviorsText = GameObject.FindGameObjectWithTag("AllBehaviors").GetComponent<Text>();
        }

        GameObject[] uniqueUtilityBehaviors = new GameObject[utilityBehaviors.Length];
        int index = 0;

        foreach (GameObject go in utilityBehaviors)
        {
            uniqueUtilityBehaviors[index] = Instantiate(go, transform.parent);
            uniqueUtilityBehaviors[index].transform.parent = transform;
            index++;
        }

        utilityBehaviors = uniqueUtilityBehaviors;

        startBehaviorUpdateTime = behaviorUpdateTime;

        if (utilityBehaviors.Length > 0) {
            UpdateConsiderationProperties();

            foreach (var behavior in utilityBehaviors) {
                UtilityBehavior ub = behavior.GetComponent<UtilityBehavior>();
                ub.UpdateBehavior(this);
                utilScores.Add(ub.score);
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
            utilScores.Clear();
            allBehaviorsText.text = "";
            UpdateConsiderationProperties();

            foreach (var behavior in utilityBehaviors)
            {
                UtilityBehavior ub = behavior.GetComponent<UtilityBehavior>();
                utilScores.Add(ub.UpdateBehavior(this));
                allBehaviorsText.text += behavior.name + ": " + ub.score + "\n";
            }

            UtilityBehavior highestUtilityBehavior = GetHighestUtility();
            if ((currentUtilityBehavior != null && !highestUtilityBehavior.isActive)) {
                currentUtilityBehavior.Reset(this);
                currentUtilityBehavior = highestUtilityBehavior;
                currentUtilityBehavior.Trigger(this);
                currentBehaviorText.text = currentUtilityBehavior.name;
            }

            behaviorUpdateTime = startBehaviorUpdateTime;
        }
    }

    private UtilityBehavior GetHighestUtility()
    {
        float highestScore = utilScores.Max();
        if (highestScore == 0) {
            return defaultUtilityBehavior;
        }

        int[] indices = utilScores.Select((x, i) => new { Index = i, Value = x }).Where(x => x.Value == highestScore).Select(x => x.Index).ToArray();
        
        if (indices.Length > 1) {
            List<int> ranks = new List<int>();
            UtilityBehavior[] highestUtilityBehaviors = new UtilityBehavior[indices.Length];
            int highestUtilityBehaviorsIndex = 0;
            foreach (int index in indices)
            {
                UtilityBehavior ub = utilityBehaviors[index].GetComponent<UtilityBehavior>();
                ranks.Add(ub.rank);
                highestUtilityBehaviors[highestUtilityBehaviorsIndex] = ub;
                highestUtilityBehaviorsIndex++;
            }
            int highestRank = ranks.Max();

            return highestUtilityBehaviors.Where(ub => ub.rank == highestRank).First();
        }

        return utilityBehaviors[utilScores.IndexOf(highestScore)].GetComponent<UtilityBehavior>();
    }

    public void UpdateConsiderationProperties()
    {
        SensorController sensorController = GetComponent<SensorController>();
        NavigationController navigationController = GetComponent<NavigationController>();
        CombatController combatController = GetComponent<CombatController>();
        RangedWeapon rangedWeapon = combatController.itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        Attributes attributes = GetComponent<Attributes>();

        // Is player visible
        propertyList["IsPlayerVisibleConsideration"] = sensorController.objectVisible ? 1.00f : 0.00f;

        // Health
        propertyList["HealthConsideration"] = attributes.health / attributes.maxHealth;

        // Ammo
        propertyList["AmmoConsideration"] = rangedWeapon.ammo / rangedWeapon.maxAmmo;

        // Search
        propertyList["SearchConsideration"] = navigationController.searchTime / navigationController.maxSearchTime;

        // Can Attack
        propertyList["CanAttackConsideration"] = combatController.available ? 1.00f : 0.00f;

        // Last position known
        propertyList["IsLastPositionKnownConsideration"] = navigationController.lastKnownPosition.HasValue ? 1.00f : 0.00f;
    }
}
