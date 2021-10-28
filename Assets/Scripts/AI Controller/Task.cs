using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public enum TaskName {
        Jump
    }

    public TaskName taskName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTask()
    {
        switch (taskName) {
            case TaskName.Jump:
                TaskJump();
                break;
        }
    }

    public void TaskJump()
    {
        Debug.Log("Jump!");
    }
}
