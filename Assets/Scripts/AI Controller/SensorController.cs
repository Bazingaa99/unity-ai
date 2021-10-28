using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SensorController : MonoBehaviour
{
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
    public event EventHandler<OnPathBlockedEventArgs> onPathBlocked;
    public class OnPathBlockedEventArgs : EventArgs {
        public GameObject blockerObject;
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
            return false;
        }

        if (LayerMask.LayerToName(obj.layer) == "Door" && agent.hasPath) {
            return isBlockingPath(origin, obj);
        }

        return true;
    }

    private bool isBlockingPath(Vector3 origin, GameObject obj) {
        var hitInfo = new RaycastHit();
        if (pathBlocker == null && Physics.Linecast(origin, agent.path.corners[1], out hitInfo, pathBlockers)) {
            if (hitInfo.transform == obj.transform) {
                pathBlocker = obj;
                onPathBlocked?.Invoke(this, new OnPathBlockedEventArgs { blockerObject = pathBlocker });
                return true;    
            }
        } else if (pathBlocker == obj) {
            return true;
        }

        return false;
    }

    private Mesh CreateSensorMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
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
        mesh = CreateSensorMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh) {
            Gizmos.color = meshColor;
            
            Gizmos.DrawMesh(mesh, transform.position - new Vector3(0, 1, 0), transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        foreach (var obj in objects) {
            Gizmos.DrawSphere(obj.transform.position, 1f);
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
