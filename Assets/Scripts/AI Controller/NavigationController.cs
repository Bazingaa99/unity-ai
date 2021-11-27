using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class NavigationController : MonoBehaviour
{
    public Path path;
    
    public int currentPointIndex;
    public int currentObjectToLookAtIndex;

    public NavMeshAgent navMeshAgent;
    private SensorController sensorController;
    [HideInInspector]
    public Vector3? lastKnownPosition = null;

    [HideInInspector]
    public bool lookAround = false;
    [HideInInspector]
    public bool patrol = false;
    private bool patrolling = false;
    private int currentPositionIndex;
    private Vector3 lastPosition;
    [HideInInspector]
    public bool followPlayer;
    [HideInInspector]
    public bool search;
    public float lookingAroundTimer;
    private float startLookingAroundTimer;

    GameObject[] currentObjectsToLookAt;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sensorController = GetComponent<SensorController>();
        currentPointIndex = 0;
        currentObjectToLookAtIndex = 0;
        currentPositionIndex = 0;
        lastPosition = path.checkpoints[0].transform.position;
        startLookingAroundTimer = lookingAroundTimer;
    }

    void Update()
    {
        if (patrol) {
            Patrol();
        } else {
            patrolling = false;
        }

        if (followPlayer) {
            FollowPlayer();
        }

        if (search) {
            Search();
        }

        if (lookAround) {
            LookAround();
        }
    }

    public GameObject[] CreateGameObjectsAroundPosition (int num, Vector3 point, float radius){
        GameObject[] tempGameObjects = new GameObject[num + 1];

        GameObject tempGameObject = new GameObject("tempGameObject");
        GameObject rightGo = Instantiate (tempGameObject, transform.position + transform.right * radius, Quaternion.identity) as GameObject;
        tempGameObjects[0] = rightGo;

        Debug.Log("Created a gameobject");

        GameObject leftGo = Instantiate (tempGameObject, transform.position - transform.right * radius, Quaternion.identity) as GameObject;
        tempGameObjects[1] = leftGo;
        Debug.Log("Created a gameobject");


        GameObject middleGo = Instantiate (tempGameObject, transform.position + transform.forward * radius, Quaternion.identity) as GameObject;
        tempGameObjects[num] = middleGo;
        Debug.Log("Created a gameobject");

        Destroy(tempGameObject);


        Debug.DrawLine(transform.position, transform.position + transform.forward * radius, Color.green, 160f);

        return tempGameObjects;
 } 

    public void MoveToPosition(Vector3 position)
    {
        navMeshAgent.destination = position;
        lastPosition = position;
    }

    public void Patrol()
    {
        if (!patrolling) {
            MoveToPosition(lastPosition);
            patrolling = true;
        }

        float distance = Vector3.Distance(transform.position, lastPosition);

        if (distance < 1.5f) {
            if (currentPositionIndex <= path.checkpoints.Length - 1) {
                MoveToPosition(path.checkpoints[currentPositionIndex].transform.position);
                currentPositionIndex++;
            } else {
                currentPositionIndex = 0;
            }   
        }
    }

    public void FollowPlayer()
    {
        navMeshAgent.destination = sensorController.objectTransform.position;
    }

    public void Search()
    {
        float distance = Vector3.Distance(transform.position, (Vector3)lastKnownPosition);

        if (distance < 1.5f && lookingAroundTimer <= 0) {
            LookAround();
        } else {
            lookingAroundTimer -= Time.deltaTime;
        }
    }

    public void LookAround()
    {
        if (currentObjectsToLookAt == null || currentObjectsToLookAt[0] == null) {
            currentObjectsToLookAt = CreateGameObjectsAroundPosition(2, transform.position, sensorController.distance);
        }

        float dot = Vector3.Dot(transform.forward, (currentObjectsToLookAt[currentObjectToLookAtIndex].transform.position - transform.position).normalized);
        
        if (dot < 0.99f) {
            Transform target = currentObjectsToLookAt[currentObjectToLookAtIndex].transform;
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 75);
        } else if (currentObjectToLookAtIndex < currentObjectsToLookAt.Length - 1) {
            currentObjectToLookAtIndex++;
        } else {
            lookingAroundTimer = startLookingAroundTimer;
            currentObjectToLookAtIndex = 0;
        }
    }

    public void DestroyGameObjects()
    {
        if (currentObjectsToLookAt != null) {
            foreach (GameObject go in currentObjectsToLookAt)
            {
                Destroy(go);
            }
        }
    }
}