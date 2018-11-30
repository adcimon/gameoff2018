using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth = 100;

    [Header("Impact")]
    public AnimationCurve curve;
    public float damageRadius = 1f;
    public float bounceAmplitude = 1f;
    public float totalTime = 1f;

    [Header("Effects")]
    public GameObject deathEffect;

    private Material material;
    private bool isAnimating = false;
    private float currentTime = 0f;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if( isAnimating )
        {
            // Update shader parameters.
            currentTime += Time.deltaTime;
            material.SetFloat("_AnimationValue", curve.Evaluate(currentTime / totalTime) * bounceAmplitude);
        }

        if( currentTime > totalTime )
        {
            // End animation.
            currentTime = 0f;
            isAnimating = false;
        }
    }

    private void Death()
    {
        // Instantiate the death effect.
        if( deathEffect )
        {
            GameObject go = Instantiate(deathEffect);
            go.transform.position = gameObject.transform.position;
        }

        // Destroy the destructible.
        Destroy(gameObject);
    }

    public void Impact( Vector3 position, Vector3 direction )
    {
        if( curve == null || material == null )
        {
            return;
        }

        // Pass the parameters to the shader.
        material.SetVector("_ImpactPosition", transform.InverseTransformPoint(position));
        material.SetVector("_ImpactDirection", direction);
        material.SetFloat("_DamageRadius", damageRadius);
        material.SetFloat("_AnimationValue", curve.Evaluate(currentTime / totalTime) * bounceAmplitude);

        // Start animation.
        currentTime = 0f;
        isAnimating = true;
    }

    public void Damage( int damage )
    {
        // Impact animation.
        Impact(gameObject.transform.position, Vector3.right);

        // Apply damage and check death.
        currentHealth -= damage;
        if( currentHealth <= 0 )
        {
            Death();
        }
    }
}