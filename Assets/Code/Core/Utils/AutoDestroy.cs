using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 1;
    public bool runOnStart = true;

    private bool autoDestroy = false;
    private float elapsedTime = 0;

    private void Start()
    {
        if( runOnStart )
        {
            autoDestroy = true;
        }
    }

    private void Update()
    {
        if( !autoDestroy )
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if( elapsedTime >= lifetime )
        {
            Destroy(gameObject);
        }
    }
}