using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private GameObject[] currentObjectsToLookAt;
    public float playerAvoidDistance;
    private float startAgentAngularSpeed;
    private bool takingCover;
    public bool takeCover;
    private Vector3? currentCoverPosition = null;
    private NavMeshHit currentHit;
    private ConsiderationProperties considerationProperties;
    public float maxSearchTime;
    public float searchTime;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sensorController = GetComponent<SensorController>();
        considerationProperties = GetComponent<ConsiderationProperties>();
        currentPointIndex = 0;
        currentObjectToLookAtIndex = 0;
        currentPositionIndex = 0;
        lastPosition = path.checkpoints[0].transform.position;
        startLookingAroundTimer = lookingAroundTimer;
        startAgentAngularSpeed = navMeshAgent.angularSpeed;
    }

    void Update()
    {
        considerationProperties.propertyList["HasPathConsideration"] = path != null ? 1.00f : 0.00f;

        if (searchTime > 0) {
            searchTime -= Time.deltaTime;
        }

        if (patrol) {
            Patrol();
        } else {
            patrolling = false;
        }

        if (followPlayer) {
            FollowPlayer();
        }

        if (sensorController.objectVisible) {
            RotateTowardsPlayer();
        } else {
            navMeshAgent.updateRotation = true;
        }

        if (search) {
            Search();
        }

        if (lookAround) {
            LookAround();
        }

        if (takeCover) {
            TakeCover(sensorController.objectTransform);
        } else {
            takingCover = false;
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
        if (sensorController.objectTransform.position != null) {
            float distanceToPlayer = Vector3.Distance(transform.position, sensorController.objectTransform.position);

            if (Mathf.Abs(distanceToPlayer - playerAvoidDistance) < 0.25f){
                navMeshAgent.isStopped = true;
            } else if (distanceToPlayer < playerAvoidDistance) {
                navMeshAgent.isStopped = false;
                Vector3 toPlayer = sensorController.objectTransform.position - transform.position;
                Vector3 targetDirection = toPlayer.normalized * -7.5f;
                Vector3 targetPosition = transform.position + targetDirection;
                NavMeshHit navMeshHit;
                RaycastHit raycastHit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out raycastHit, 5f)) {
                    if (NavMesh.FindClosestEdge(targetPosition, out navMeshHit, NavMesh.AllAreas)) {
                        targetPosition = navMeshHit.position;
                    }
                } else if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 1.0f, NavMesh.AllAreas)) {
                    targetPosition = navMeshHit.position;
                } 
                navMeshAgent.destination = targetPosition;
            } else {
                navMeshAgent.isStopped = false;
                navMeshAgent.destination = sensorController.objectTransform.position;
            }
        } else {
            if (navMeshAgent.remainingDistance < 0.5f) {
                Vector3 randomPosition = transform.position + new Vector3(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1)) * sensorController.distance;
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(randomPosition, out navMeshHit, 1.0f, NavMesh.AllAreas)) {
                    navMeshAgent.destination = randomPosition;
                }
            }
        }
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

    public void TakeCover(Transform player)
    {
        Vector3 origin = transform.position;
        Vector3 destination = player.transform.position;
        Vector3 direction = destination - origin;

        direction.y = 0;
        origin.y += sensorController.height / 2;
        destination.y = origin.y;

        if (Physics.Linecast((Vector3) currentHit.position, player.position, sensorController.occlusionLayers)) {
            navMeshAgent.destination = currentHit.position;
        } else {
            FindCoverPosition(player);
        }
    }

    public void FindCoverPosition(Transform player)
    {
        List<NavMeshHit> hitList = new List<NavMeshHit>();
        NavMeshHit navHit;
        bool positionFound = false;

        // Loop to create random points around the player so we can find the nearest point to all of them, storting the hits in a list
        for(int i = 0; i < 15; i++) {
            Vector3 spawnPoint = transform.position;
            Vector2 offset = UnityEngine.Random.insideUnitCircle * i * 2;
            spawnPoint.x += offset.x;
            spawnPoint.z += offset.y;

            NavMesh.FindClosestEdge(spawnPoint, out navHit, NavMesh.AllAreas);

            hitList.Add(navHit);
        }

        // sort the list by distance using Linq
        var sortedList = hitList.OrderBy(x => x.distance);

        // Loop through the sortedList and see if the hit normal doesn't point towards the enemy.
        // If it doesn't point towards the enemy, navigate the agent to that position and break the loop as this is the closest cover for the agent. (Because the list is sorted on distance)
        foreach(NavMeshHit hit in sortedList) {
            if(Vector3.Dot(hit.normal, (player.transform.position - transform.position)) < 0) {
                currentHit = hit;
                lastKnownPosition = player.position;
                positionFound = true;
            }
        }

        if (!positionFound) {
            FindCoverPosition(player);
        }
    }

    private void RotateTowardsPlayer()
    {        
        navMeshAgent.updateRotation = false;
        Vector3 toPlayer = sensorController.objectTransform.position - transform.position;
        toPlayer.y = 0;
        Quaternion rotation = Quaternion.LookRotation(toPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 75);
    }
}