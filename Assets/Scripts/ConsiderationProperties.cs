using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsiderationProperties : MonoBehaviour
{
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();

    void Start()
    {
        // GameObject[] considerations = Resources.LoadAll<GameObject>("Prefabs/Considerations");
        // foreach (GameObject consideration in considerations)
        // {
        //     propertyList.Add(consideration.name, 0.00f);
        // }
    }
}
