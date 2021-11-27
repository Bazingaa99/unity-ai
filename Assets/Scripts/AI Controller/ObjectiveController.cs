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
    }
}
