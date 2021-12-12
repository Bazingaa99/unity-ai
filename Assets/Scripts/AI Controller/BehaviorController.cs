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
    public float startBehaviorUpdateTime;

    private Text currentBehaviorText;
    private Text allBehaviorsText;
    public ConsiderationProperties considerationProperties;

    void Start()
    {
        if (debug) {
            currentBehaviorText = GameObject.FindGameObjectWithTag("CurrentBehavior").GetComponent<Text>();
            allBehaviorsText = GameObject.FindGameObjectWithTag("AllBehaviors").GetComponent<Text>();
        }

        considerationProperties = GetComponent<ConsiderationProperties>();

        startBehaviorUpdateTime = behaviorUpdateTime;

        if (utilityBehaviors.Length > 0) {
            UpdateConsiderationProperties();

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
            UpdateConsiderationProperties();

            foreach (var behavior in utilityBehaviors)
            {
                utilWeights.Add(behavior.UpdateBehavior(this));
                allBehaviorsText.text += behavior.name + ": " + behavior.weight + "\n";
            }

            if ((currentUtilityBehavior != null && currentUtilityBehavior != GetHighestUtility())) {
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
            UtilityBehavior[] highestUtilityBehaviors = new UtilityBehavior[indices.Length];
            int highestUtilityBehaviorsIndex = 0;
            foreach (int index in indices)
            {
                ranks.Add(utilityBehaviors[index].rank);
                highestUtilityBehaviors[highestUtilityBehaviorsIndex] = utilityBehaviors[index];
                highestUtilityBehaviorsIndex++;
            }
            int highestRank = ranks.Max();

            return highestUtilityBehaviors.Where(ub => ub.rank == highestRank).First();
        }

        return utilityBehaviors[utilWeights.IndexOf(highestWeight)];
    }

    public void UpdateConsiderationProperties()
    {
        SensorController sensorController = GetComponent<SensorController>();
        NavigationController navigationController = GetComponent<NavigationController>();
        CombatController combatController = GetComponent<CombatController>();
        RangedWeapon rangedWeapon = combatController.itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        Attributes attributes = GetComponent<Attributes>();

        // Is player visible
        considerationProperties.propertyList["IsPlayerVisibleConsideration"] = sensorController.objectVisible ? 1.00f : 0.00f;

        // In Player Sight
        considerationProperties.propertyList["InPlayerSightConsideration"] = 0.00f;

        // Health
        considerationProperties.propertyList["HealthConsideration"] = attributes.health / attributes.maxHealth;

        // Has Path
        considerationProperties.propertyList["HasPathConsideration"] = navigationController.path != null ? 1.00f : 0.00f;

        // Ammo
        considerationProperties.propertyList["AmmoConsideration"] = rangedWeapon.ammo / rangedWeapon.maxAmmo;

        // Energy
        considerationProperties.propertyList["EnergyConsideration"] = attributes.energy / attributes.maxEnergy;

        // Search
        considerationProperties.propertyList["SearchConsideration"] = navigationController.searchTime / navigationController.maxSearchTime;

        // Can Attack
        considerationProperties.propertyList["CanAttack"] = combatController.available ? 1.00f : 0.00f;
    }
}
