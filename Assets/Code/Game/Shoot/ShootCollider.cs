using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ShootCollider : MonoBehaviour
{
    public GameObject owner;

    private Health health;

    private void Start()
    {
        owner = gameObject.transform.parent.gameObject;
        health = owner.GetComponent<Health>();
    }

    public void Damage( GameObject source, int damage )
    {
        if( !health )
        {
            return;
        }

        health.Damage(damage);
    }
}