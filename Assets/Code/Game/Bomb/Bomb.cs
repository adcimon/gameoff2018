using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public int damage = 50;
    public float duration = 3;
    public Text text;
    public List<GameObject> effects = new List<GameObject>();

    private ExplosionCollider explosionCollider;
    private float elapsedTime = 0;

    private void Start()
    {
        explosionCollider = gameObject.transform.Find("ExplosionCollider").gameObject.GetComponent<ExplosionCollider>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Update UI.
        if( text )
        {
            text.text = Mathf.Ceil(duration - elapsedTime).ToString();
        }

        if( elapsedTime >= duration )
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        for( int i = 0; i < effects.Count; i++ )
        {
            GameObject go = Instantiate(effects[i]);
            go.transform.position = gameObject.transform.position;
        }

        explosionCollider.Damage(damage);

        Destroy(gameObject);
    }
}