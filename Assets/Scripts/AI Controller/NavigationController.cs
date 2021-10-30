using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class NavigationController : MonoBehaviour
{
    public Path path;
    private GameObject objective;
    
    public int currentPointIndex;

    public NavMeshAgent navMeshAgent;
    private bool pathBlocked = false;
    public bool stop;
    private bool pathFinished;
    public float idleDistance;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentPointIndex = 0;
    }

    void Start()
    {
        ObjectiveController objectiveController = GetComponent<ObjectiveController>();
        objectiveController.OnClearPathObjectiveCreated += ObjectiveController_OnClearPathObjectiveCreated;

        BehaviorController behaviorController = GetComponent<BehaviorController>();
        behaviorController.OnContinuePreviousPath += BehaviorController_OnContinuePreviousPath;
    }

    private IEnumerator MoveToObjective()
    {
        while (navMeshAgent.remainingDistance > 0.1f || navMeshAgent.pathPending) {
            yield return null;
        }
    }

    private void ObjectiveController_OnClearPathObjectiveCreated(object sender, ObjectiveController.OnClearPathObjectiveCreatedEventArgs e)
    {
        pathBlocked = true;
        objective = e.button;
        navMeshAgent.destination = objective.transform.position;
        
        StartCoroutine(MoveToObjective());
    }

    private void BehaviorController_OnContinuePreviousPath(object sender, EventArgs e)
    {
        
    }

    public void Follow(Transform transform)
    {
        navMeshAgent.destination = transform.position;
    }

    public void StopFollow()
    {
        navMeshAgent.ResetPath();
    }
}