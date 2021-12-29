using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SearchBehavior : UtilityBehavior, ISerializationCallbackReceiver
{    
    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.MoveToPosition((Vector3) navigationController.lastKnownPosition);
        navigationController.search = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        score = 0;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.search = false;
        navigationController.lastKnownPosition = null;
        navigationController.DestroyGameObjects();
    }
}
