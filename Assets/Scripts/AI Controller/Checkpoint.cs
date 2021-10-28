using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{
    public GameObject task;
    private class CheckpointEventArgs : EventArgs {
        public GameObject agentObject;
        public CheckpointType checkpointType;
        public GameObject taskObject;
    }
    public enum CheckpointType {
        TASK,
        PASS,
        IDLE
    }
    public CheckpointType type;
    public float idleTime;
}
