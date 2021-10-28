using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectiveController : MonoBehaviour
{
    public event EventHandler<OnClearPathObjectiveCreatedEventArgs> OnClearPathObjectiveCreated;
    public class OnClearPathObjectiveCreatedEventArgs : EventArgs {
        public GameObject button;
    }
    void Start()
    {
        SensorController sensorController = GetComponent<SensorController>();
        sensorController.onPathBlocked += SensorController_OnPathBlocked;
    }

    private void SensorController_OnPathBlocked(object sender, SensorController.OnPathBlockedEventArgs e)
    {
        // var buttonObject = e.blockerObject.GetComponent<DoorController>().button;
        // AIController.Objective objective = new AIController.Objective { name = "Open Door", currentObjective = true, isCompleted = false };
        // var aiController = GetComponent<AIController>();
        // aiController.currentObjective = objective;

        // OnClearPathObjectiveCreated?.Invoke(this, new OnClearPathObjectiveCreatedEventArgs { button = buttonObject });
    }
}
