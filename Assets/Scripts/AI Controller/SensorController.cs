using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SensorController : MonoBehaviour
{
    public bool debug;
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;

    public Color meshColor = Color.red;

    public LayerMask layerMask;
    public LayerMask occlusionLayers;
    public LayerMask pathBlockers;
    private Mesh mesh;
    private NavMeshAgent agent;
    private GameObject pathBlocker;
    public List<GameObject> objects = new List<GameObject>();

    [HideInInspector]
    public Transform objectTransform = null;
    private GameObject player;
    public bool objectVisible = false;
    private BehaviorController behaviorController;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        behaviorController = GetComponent<BehaviorController>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Scan(); 
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance, layerMask);

        objects.Clear();
        foreach (Collider collider in hitColliders)
        {
            GameObject obj = collider.gameObject;

            if (IsInSight(obj)) {
                objects.Add(obj);
            }
        }

        if (!objects.Contains(player)) {
            objectVisible = false;
        }
    }

    private bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < 0 || direction.y > height) {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle) {
            return false;
        }

        origin.y += height / 2;
        destination.y = origin.y;
        if (Physics.Linecast(origin, destination, occlusionLayers)) {
            if (LayerMask.LayerToName(obj.layer) == "Player" && objectVisible) {
                objectVisible = false;
            }
            return false;
        }

        if (LayerMask.LayerToName(obj.layer) == "Player" && !objectVisible) {
            objectTransform = obj.transform;
            objectVisible = true;
        }

        return true;
    }

    private Mesh CreateSensorMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 12;
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++) {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height; 

            currentAngle += deltaAngle;

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // topCenter
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
        }

        for (int i = 0; i < numVertices; i++) {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        if (debug) {
            mesh = CreateSensorMesh();
        } else {
            mesh = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (debug) {
            if (mesh) {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(mesh, transform.position - new Vector3(0, 1, 0), transform.rotation);

                // Gizmos.color = Color.red;
                // Gizmos.DrawWireMesh(mesh, transform.position - new Vector3(0, 1, 0), transform.rotation);
            }

            agent = GetComponent<NavMeshAgent>();

            var path = agent.path;

            Gizmos.color = Color.red;
            foreach (var corner in path.corners)
            {
                Gizmos.DrawSphere(corner, 1f);
            }
        }
    }
}
