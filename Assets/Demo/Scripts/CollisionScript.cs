using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public float baseDamage;
    public LayerMask objectsToAffect;
    public LayerMask obstacles;
    [HideInInspector]
    public GameObject launcher;

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;
        Transform collidedObjectParent = collidedObject.transform.parent;

        if (collidedObject != null) {
            if ((objectsToAffect == (objectsToAffect | (1 << collidedObject.layer))) && collidedObjectParent != null && collidedObjectParent.gameObject != launcher) {
            Attributes attributes = collidedObject.GetComponentInParent<Attributes>();
            attributes.health -= baseDamage;
            if (attributes.health <= 0) {
                Destroy(collidedObject.transform.parent.gameObject);
            }
            
            GameObject.Destroy(gameObject);
            } else if (obstacles == (obstacles | (1 << collidedObject.layer))) {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
