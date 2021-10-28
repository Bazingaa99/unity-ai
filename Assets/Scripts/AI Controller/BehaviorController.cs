using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehaviorController : MonoBehaviour
{
    public enum BehaviorStatus {
        None,
        Idle,
        Attack,
        Follow,
        Runaway
    }

    private BehaviorStatus status;
    public event EventHandler OnContinuePreviousPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StayIdle(float idleTime)
    {
        status = BehaviorStatus.Idle;
        StartCoroutine(Idle(idleTime));
    }

    public IEnumerator Idle(float idleTime)
    {
        while (idleTime >= 0.0f) {
            idleTime -= Time.deltaTime;
            yield return null;
        }

        status = BehaviorStatus.None;
        OnContinuePreviousPath?.Invoke(this, EventArgs.Empty);
    }

    public void Attack()
    {

    }

    public void Follow()
    {

    }

    public void RunAway()
    {

    }
}
