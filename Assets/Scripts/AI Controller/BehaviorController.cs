using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BehaviorController : MonoBehaviour
{
    private bool debug;
    public GameObject[] utilityBehaviors;
    public GameObject defaultUtilityBehavior;
    public List<float> utilScores = new List<float>();
    public UtilityBehavior currentUtilityBehavior;
    private float behaviorUpdateTime = 0.1f;

    [HideInInspector]
    public float startBehaviorUpdateTime;
    private Text currentBehaviorText;
    private Text allBehaviorsText;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    private Dropdown dropdown;
    private Material[] materials = new Material[2];

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("DebugUI") != null) {
            currentBehaviorText = GameObject.FindGameObjectWithTag("CurrentBehavior").GetComponent<Text>();
            allBehaviorsText = GameObject.FindGameObjectWithTag("AllBehaviors").GetComponent<Text>();
            dropdown = GameObject.FindGameObjectWithTag("Dropdown").GetComponent<Dropdown>();

            dropdown.onValueChanged.AddListener(UpdateDebug);
        }

        GameObject[] uniqueUtilityBehaviors = new GameObject[utilityBehaviors.Length];
        int index = 0;

        // Need to instantiate the passed utility behaviors, so that each agent would have a unique set of utility behaviors.
        foreach (GameObject go in utilityBehaviors)
        {
            uniqueUtilityBehaviors[index] = Instantiate(go, transform.parent);
            uniqueUtilityBehaviors[index].transform.parent = transform;
            index++;
        }

        utilityBehaviors = uniqueUtilityBehaviors;
        defaultUtilityBehavior = Instantiate(defaultUtilityBehavior, transform.parent);
        defaultUtilityBehavior.transform.parent = transform;

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
            if (debug) {
                currentBehaviorText.text = currentUtilityBehavior.name;
            }
        }
    }

    void Update()
    {
        if (behaviorUpdateTime >= 0) {
            behaviorUpdateTime -= Time.deltaTime;
        } else {
            utilScores.Clear();
            if (debug) {
                allBehaviorsText.text = "";
            }
            
            UpdateConsiderationProperties();

            foreach (var behavior in utilityBehaviors)
            {
                UtilityBehavior ub = behavior.GetComponent<UtilityBehavior>();
                utilScores.Add(ub.UpdateBehavior(this));
                if (debug) {
                    allBehaviorsText.text += behavior.name + ": " + ub.score + "\n";
                }
            }

            UtilityBehavior highestUtilityBehavior = GetHighestUtility();
            if ((currentUtilityBehavior != null && !highestUtilityBehavior.isActive)) {
                currentUtilityBehavior.Reset(this);
                currentUtilityBehavior = highestUtilityBehavior;
                currentUtilityBehavior.Trigger(this);
                if (debug) {
                    currentBehaviorText.text = currentUtilityBehavior.name;
                }
            }

            behaviorUpdateTime = startBehaviorUpdateTime;
        }
    }

    private UtilityBehavior GetHighestUtility()
    {
        float highestScore = utilScores.Max();
        if (highestScore == 0) {
            return defaultUtilityBehavior.GetComponent<UtilityBehavior>();
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

        propertyList["IsPlayerVisibleConsideration"] = sensorController.objectVisible ? 1.00f : 0.00f;
        propertyList["HealthConsideration"] = attributes.health / attributes.maxHealth;
        propertyList["AmmoConsideration"] = rangedWeapon.ammo / rangedWeapon.maxAmmo;
        propertyList["SearchConsideration"] = navigationController.searchTime / navigationController.maxSearchTime;
        propertyList["CanAttackConsideration"] = combatController.available ? 1.00f : 0.00f;
        propertyList["IsLastPositionKnownConsideration"] = navigationController.lastKnownPosition.HasValue ? 1.00f : 0.00f;
    }

    public void UpdateDebug(int agent)
    {
        if (name == "Agent " + agent.ToString()) {
            debug = true;
            currentBehaviorText.text = currentUtilityBehavior.name;
            GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        } else {
            debug = false;
            GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        }
    }
}
