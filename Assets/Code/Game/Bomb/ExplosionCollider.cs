using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExplosionCollider : MonoBehaviour
{
    private List<GameObject> enteredGameObjects = new List<GameObject>();

    private void OnTriggerEnter2D( Collider2D other )
    {
        //Debug.Log(other.gameObject.name + " entered " + gameObject.name + ".\n");

        enteredGameObjects.Add(other.gameObject);
    }

    private void OnTriggerExit2D( Collider2D other )
    {
        //Debug.Log(other.gameObject.name + " exited " + gameObject.name + ".\n");

        enteredGameObjects.Remove(other.gameObject);
    }

    public void Damage( int damage )
    {
        for( int i = 0; i < enteredGameObjects.Count; i++ )
        {
            GameObject go = enteredGameObjects[i];
            if( !go )
            {
                return;
            }

            // Check if a destructible is inside.
            Destructible destructible = go.GetComponent<Destructible>();
            if( destructible )
            {
                destructible.Damage(damage);
            }

            // Check if a player is inside.
            Health health = go.GetComponent<Health>();
            if( health )
            {
                health.Damage(damage);
            }
        }
    }
}