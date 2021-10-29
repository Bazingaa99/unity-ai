using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class NavigationController : MonoBehaviour
{
    public Path path;
    private GameObject objective;
    
    private int currentPointIndex;

    private NavMeshAgent navMeshAgent;
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

        if (path != null && path.checkpoints.Length > 0) {
            StartPath();
        }
    }

    private void StartPath()
    {
        navMeshAgent.destination = path.checkpoints[currentPointIndex].transform.position;
        path.currentCheckpoint = path.checkpoints[currentPointIndex];
        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath() 
    {
        while ((navMeshAgent.remainingDistance > 0.1f || navMeshAgent.pathPending) && !pathBlocked && !stop) {
            yield return null;
        }

        if (!pathBlocked && !stop) {
            if (currentPointIndex < path.checkpoints.Length - 1) {
                navMeshAgent.destination = path.checkpoints[++currentPointIndex].transform.position;
                StartCoroutine(FollowPath());
            } else {
                switch (path.type){
                    case Path.PathType.Stop:
                        navMeshAgent.isStopped = true;
                        break;
                    case Path.PathType.GoBack:
                        Array.Reverse(path.checkpoints);
                        currentPointIndex = 0;
                        path.type = Path.PathType.Stop;
                        StartCoroutine(FollowPath());
                        break;
                    case Path.PathType.BackAndForthPatrol:
                        Array.Reverse(path.checkpoints);
                        currentPointIndex = 0;
                        StartCoroutine(FollowPath());
                        break;
                    case Path.PathType.CirclePatrol:
                        currentPointIndex = 0;
                        navMeshAgent.destination = path.checkpoints[currentPointIndex].transform.position;
                        StartCoroutine(FollowPath());
                        break;
                }
            }
        }
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
        stop = false;
        StartPath();
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