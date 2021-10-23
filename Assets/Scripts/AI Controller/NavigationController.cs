using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class NavigationController : MonoBehaviour
{
    public Transform[] pathPoints;

    public enum AgentPathBehavior {
        Stop,
        GoBack,
        BackAndForthPatrol,
        CirclePatrol
    }
    public AgentPathBehavior agentPathBehavior;
    
    private int currentPointIndex;

    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentPointIndex = 0;
    }

    void Start()
    {
        navMeshAgent.destination = pathPoints[currentPointIndex].position;
        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath() {

        while (navMeshAgent.remainingDistance > 0.1f || navMeshAgent.pathPending) {
            yield return null;
        }

        if (currentPointIndex < pathPoints.Length - 1) {
            navMeshAgent.destination = pathPoints[++currentPointIndex].position;
            StartCoroutine(FollowPath());
        } else {
            switch (agentPathBehavior){
                case AgentPathBehavior.Stop:
                    navMeshAgent.isStopped = true;
                    break;
                case AgentPathBehavior.GoBack:
                    Array.Reverse(pathPoints);
                    currentPointIndex = 0;
                    agentPathBehavior = AgentPathBehavior.Stop;
                    StartCoroutine(FollowPath());
                    break;
                case AgentPathBehavior.BackAndForthPatrol:
                    Array.Reverse(pathPoints);
                    currentPointIndex = 0;
                    StartCoroutine(FollowPath());
                    break;
                case AgentPathBehavior.CirclePatrol:
                    currentPointIndex = 0;
                    navMeshAgent.destination = pathPoints[currentPointIndex].position;
                    StartCoroutine(FollowPath());
                    break;
            }
        }
    }
}