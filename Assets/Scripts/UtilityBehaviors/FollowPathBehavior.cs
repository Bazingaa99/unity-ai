using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowPathBehavior : UtilityBehavior
{
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        weight = navigationController.path.weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        StartPath(behaviorController.GetComponent<NavigationController>());
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        StopFollow(behaviorController.GetComponent<NavigationController>());
    }

    private void StartPath(NavigationController navigationController)
    {
        navigationController.navMeshAgent.destination = navigationController.path.checkpoints[navigationController.currentPointIndex].transform.position;
        navigationController.path.currentCheckpoint = navigationController.path.checkpoints[navigationController.currentPointIndex];
        StartCoroutine(FollowPath(navigationController));
    }

    private IEnumerator FollowPath(NavigationController navigationController) 
    {
        while ((navigationController.navMeshAgent.remainingDistance > 0.1f || navigationController.navMeshAgent.pathPending) && isActive) {
            yield return null;
        }

        if (isActive) {
            if (navigationController.currentPointIndex < navigationController.path.checkpoints.Length - 1) {
                navigationController.navMeshAgent.destination = navigationController.path.checkpoints[++navigationController.currentPointIndex].transform.position;
                StartCoroutine(FollowPath(navigationController));
            } else {
                switch (navigationController.path.type){
                    case Path.PathType.Stop:
                        navigationController.navMeshAgent.isStopped = true;
                        break;
                    case Path.PathType.GoBack:
                        Array.Reverse(navigationController.path.checkpoints);
                        navigationController.currentPointIndex = 0;
                        navigationController.path.type = Path.PathType.Stop;
                        StartCoroutine(FollowPath(navigationController));
                        break;
                    case Path.PathType.BackAndForthPatrol:
                        Array.Reverse(navigationController.path.checkpoints);
                        navigationController.currentPointIndex = 0;
                        StartCoroutine(FollowPath(navigationController));
                        break;
                    case Path.PathType.CirclePatrol:
                        navigationController.currentPointIndex = 0;
                        navigationController.navMeshAgent.destination = navigationController.path.checkpoints[navigationController.currentPointIndex].transform.position;
                        StartCoroutine(FollowPath(navigationController));
                        break;
                }
            }
        }
    }

    private void StopFollow(NavigationController navigationController)
    {
        navigationController.StopFollow();
    }
}
