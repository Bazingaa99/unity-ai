using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public float baseDamage;
    public GameObject flyingParticles;
    public GameObject collisionParticles;
    public LayerMask objectsToAffect;
    public LayerMask obstacles;

    void Awake()
    {
        // flyingParticles = Instantiate(flyingParticles, transform.position, Quaternion.identity);
        // flyingParticles.transform.parent = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Destroy(flyingParticles);

        //Instantiate(collisionParticles, transform.position, Quaternion.identity);

        GameObject collidedObject = other.gameObject;

        if ((objectsToAffect.value & (1 << collidedObject.layer)) > 0) {
            Attributes attributes = collidedObject.GetComponent<Attributes>();
            attributes.health -= baseDamage;
            
            GameObject.Destroy(gameObject);
        } else if ((obstacles.value & (1 << collidedObject.layer)) > 0) {
            GameObject.Destroy(gameObject);
        }
    }
}
