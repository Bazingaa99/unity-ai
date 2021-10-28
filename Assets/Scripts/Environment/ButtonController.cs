using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject door;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        door.GetComponent<DoorController>().Open();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (LayerMask.LayerToName(other.gameObject.layer) == "AI") {
            //var aiController = other.gameObject.GetComponent<AIController>();
            //Debug.Log("layeris");
            //if (aiController.currentObjective.name == "Open Door") {
                Debug.Log("sadasda");
                OpenDoor();
            //}
        //}
    }
}
