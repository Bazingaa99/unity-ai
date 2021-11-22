using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class NavigationController : MonoBehaviour
{
    public Path path;
    private GameObject objective;
    
    public int currentPointIndex;
    public int currentObjectToLookAtIndex;
    private Vector3 currentObjectPosition;
    public bool followObject = false;

    public NavMeshAgent navMeshAgent;
    private bool pathBlocked = false;
    public bool stop;
    private bool pathFinished;
    public float idleDistance;
    private SensorController sensorController;
    public Vector3? lastKnownPosition = null;

    public bool followPath;
    public bool lookAround = false;
    private bool lookingAround = false;
    private Quaternion startingRotation;

    GameObject[] currentObjectsToLookAt;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sensorController = GetComponent<SensorController>();
        currentPointIndex = 0;
        currentObjectToLookAtIndex = 0;
    }

    public void StartMovingToPosition(Vector3? position = null)
    {
        if (position != null) {
            navMeshAgent.destination = (Vector3) position;
            StartCoroutine(MoveToPosition());
        } else {
            StartCoroutine(MoveToPosition());
        }
    }

    private IEnumerator MoveToPosition()
    {
        while (navMeshAgent.remainingDistance > 0.1f || navMeshAgent.pathPending) {
            yield return null;
        }

        if (lookAround) {
            StartLookingAround();
        }
    }

    public void StartFollow(Transform transform)
    {
        navMeshAgent.destination = transform.position;
        currentObjectPosition = transform.position;
        StartCoroutine(FollowObject(transform));
    }

    private IEnumerator FollowObject(Transform transform) {
        while (currentObjectPosition == transform.position && followObject) {
            yield return null;
        }

        if (followObject) {
            navMeshAgent.destination = transform.position;
            currentObjectPosition = transform.position;
            StartCoroutine(FollowObject(transform));
        }
    }

    public bool hasReachedDestination()
    {
        if (!navMeshAgent.pathPending){
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void StopFollow()
    {
        navMeshAgent.ResetPath();
        
    }

    public void StartPath(Path path)
    {
        if (path != null) {
            navMeshAgent.destination = path.checkpoints[currentPointIndex].transform.position;
            path.currentCheckpoint = path.checkpoints[currentPointIndex];
            StartCoroutine(FollowPath(path));
        }   
    }

    private IEnumerator FollowPath(Path path)
    {
        while ((navMeshAgent.remainingDistance > 0.1f || navMeshAgent.pathPending) && followPath) {
            yield return null;
        }

        if (followPath) {
            if (currentPointIndex < path.checkpoints.Length - 1) {
                navMeshAgent.destination = path.checkpoints[++currentPointIndex].transform.position;
                StartCoroutine(FollowPath(path));
            } else {
                switch (path.type){
                    case Path.PathType.Stop:
                        navMeshAgent.isStopped = true;
                        break;
                    case Path.PathType.GoBack:
                        Array.Reverse(path.checkpoints);
                        currentPointIndex = 0;
                        path.type = Path.PathType.Stop;
                        StartCoroutine(FollowPath(path));
                        break;
                    case Path.PathType.BackAndForthPatrol:
                        Array.Reverse(path.checkpoints);
                        currentPointIndex = 0;
                        StartCoroutine(FollowPath(path));
                        break;
                    case Path.PathType.CirclePatrol:
                        currentPointIndex = 0;
                        navMeshAgent.destination = path.checkpoints[currentPointIndex].transform.position;
                        StartCoroutine(FollowPath(path));
                        break;
                }
            }
        }
    }

    public void StartLookingAround(GameObject[] objectsToLookAt = null)
    {
        if (!lookingAround) {
            lookingAround = true;
            if (currentObjectsToLookAt != null) {
                foreach (GameObject go in currentObjectsToLookAt)
                {
                    Destroy(go);
                }
            }

            startingRotation = transform.rotation;
            currentObjectToLookAtIndex = 0;
            if (objectsToLookAt != null && objectsToLookAt.Length > 0) {
                Transform target = objectsToLookAt[0].transform;
                currentObjectsToLookAt = objectsToLookAt;
                    
                var lookPos = target.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                StartCoroutine(LookAround(objectsToLookAt, rotation));
            } else {
                Vector3 currentPosition = gameObject.transform.position;
                GameObject[] tempGameObjects = CreateGameObjectsAroundPosition(2, currentPosition, sensorController.distance);
                currentObjectsToLookAt = tempGameObjects;
                Transform target = tempGameObjects[0].transform;
                
                var lookPos = target.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                StartCoroutine(LookAround(tempGameObjects, rotation));
            }
        }
        
    }

    public GameObject[] CreateGameObjectsAroundPosition (int num, Vector3 point, float radius){
        GameObject[] tempGameObjects = new GameObject[num + 1];

        // Pasilieku sita kodo dali, jeigu rasciau laiko padaryti taip, kad useris pats galetu pasirinkti kiek tasku AI gali aplink save ziureti.
        // for (int i = 0; i < num; i++){
            
        //     /* Distance around the circle */  
        //     float radians = 2 * Mathf.PI / num * i;
            
        //     /* Get the vector direction */ 
        //     float vertical = Mathf.Sin(radians);
        //     float horizontal = Mathf.Cos(radians); 
            
        //     Vector3 spawnDir = new Vector3 (horizontal, 0, vertical);
            
        //     /* Get the spawn position */ 
        //     Vector3 spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point
            
        //     /* Now spawn */
        //     GameObject go = Instantiate (new GameObject("tempGameObject"), spawnPos, Quaternion.identity) as GameObject;
            
        //     /* Adjust height */
        //     go.transform.Translate (new Vector3 (0, go.transform.localScale.y / 2, 0));

        //     tempGameObjects[i] = go;

        //     Debug.DrawLine(transform.position, spawnPos, Color.red, 160f);
        // }

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

    private IEnumerator LookAround(GameObject[] objectsToLookAt, Quaternion rotation)
    {
        float dot = Vector3.Dot(transform.forward, (objectsToLookAt[currentObjectToLookAtIndex].transform.position - transform.position).normalized);
        
        while (dot < 0.99f && lookAround) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 75);
            dot = Vector3.Dot(transform.forward, (objectsToLookAt[currentObjectToLookAtIndex].transform.position - transform.position).normalized);
            yield return null;
        }

        if (lookAround) {
            currentObjectToLookAtIndex++;
            if (!(currentObjectToLookAtIndex <= objectsToLookAt.Length - 1)) {
                yield return new WaitForSeconds(3f);
                currentObjectToLookAtIndex = 0;
            }

            Transform target = objectsToLookAt[currentObjectToLookAtIndex].transform;
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos);
            StartCoroutine(LookAround(objectsToLookAt, rotation));
        }

        lookingAround = false;
    }
}