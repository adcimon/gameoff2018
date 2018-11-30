using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifetime;
    public bool destroyOnCollision = true;
    public GameObject owner;

    private float elapsedTime = 0;

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        // Destroy the gameobject if its lifetime has ended.
        if( elapsedTime >= lifetime )
        {
            Destroy(gameObject);
        }

        // Move the gameobject in the facing direction.
        transform.position += transform.right * speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D( Collider2D other )
    {
        //Debug.Log("Projectile " + gameObject.name + " collided with " + other.gameObject.name + ".\n");

        Destructible destructible = other.gameObject.GetComponent<Destructible>();
        if( destructible )
        {
            // Damage the destructible.
            destructible.Damage(damage);

            // Destroy the projectile.
            if( destroyOnCollision )
            {
                Destroy(gameObject);
            }
        }

        ShootCollider collider = other.gameObject.GetComponent<ShootCollider>();
        if( collider )
        {
            // Disable auto damage.
            if( owner == collider.owner )
            {
                return;
            }

            // Update the health.
            collider.Damage(owner, damage);

            // Destroy the projectile.
            if( destroyOnCollision )
            {
                Destroy(gameObject);
            }
        }
    }
}