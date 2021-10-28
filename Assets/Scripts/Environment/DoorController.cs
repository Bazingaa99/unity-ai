using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public GameObject button;
    private MeshRenderer meshRenderer;
    public Material doorOpenState;
    public Material doorClosedState;
    private NavMeshObstacle navMeshObstacle;
    // Start is called before the first frame update
    void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        meshRenderer.material = doorOpenState;
        navMeshObstacle.enabled = false;
    }

    public void Close()
    {
        meshRenderer.material = doorClosedState;
    }
}
