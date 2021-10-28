using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : MonoBehaviour
{
    private InteractionController interactionController;
    private NavigationController navigationController;
    private SensorController sensorController;
    private BehaviorController behaviorController;
    void Start()
    {
        interactionController = GetComponent<InteractionController>();
        navigationController = GetComponent<NavigationController>();
        sensorController = GetComponent<SensorController>();
        behaviorController = GetComponent<BehaviorController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject checkpointGo = other.gameObject;
        Checkpoint checkpoint = checkpointGo.GetComponent<Checkpoint>();

        if (checkpoint != null) {
            switch (checkpoint.type) {
                case Checkpoint.CheckpointType.TASK:
                    GameObject task = checkpoint.task;
                    if (task != null) {
                        Task taskController = task.GetComponent<Task>();
                        taskController.startTask();
                    }
                    break;
                case Checkpoint.CheckpointType.IDLE:
                    navigationController.stop = true;
                    behaviorController.StayIdle(checkpoint.idleTime);
                    break;
                case Checkpoint.CheckpointType.PASS:
                    break;
            }
        }
    }
}
