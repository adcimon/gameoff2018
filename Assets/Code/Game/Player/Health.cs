using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Serializable]
    public class OnDeath : UnityEvent<GameObject> { }

    [Header("Health")]
    public int currentHealth = 200;

    [Header("Effects")]
    public List<GameObject> damageEffects = new List<GameObject>();
    public List<GameObject> deathEffects = new List<GameObject>();

    [Header("UI")]
    public Text text;
    public string deathPhrase = "RIP";

    [Space(10)]
    public OnDeath onDeath;

    private void Start()
    {
        UpdateUI();
    }

    private void InstantiateEffect()
    {
        if( damageEffects.Count == 0 )
        {
            return;
        }

        GameObject go = Instantiate(damageEffects.PickRandom());

        // Set gameobject parent.
        go.transform.SetParent(gameObject.transform);

        // Set gameobject position.
        go.transform.position = gameObject.transform.position;
    }

    private void UpdateUI()
    {
        if( !text )
        {
            return;
        }

        text.text = currentHealth.ToString();
    }

    private void Death()
    {
        // Throw event.
        onDeath.Invoke(gameObject);

        // Instantiate effects.
        for( int i = 0; i < deathEffects.Count; i++ )
        {
            GameObject go = Instantiate(deathEffects[i]);
            go.transform.position = gameObject.transform.position;
        }

        // Update UI.
        text.text = deathPhrase;
        text.transform.parent.GetComponent<CopyTransform>().positionOffset = Vector3.zero;

        // Disable the gameobject.
        gameObject.SetActive(false);
    }

    public void Damage( int damage )
    {
        currentHealth -= damage;
        UpdateUI();
        InstantiateEffect();

        // Check death.
        if( currentHealth <= 0 )
        {
            Death();
        }
    }
}