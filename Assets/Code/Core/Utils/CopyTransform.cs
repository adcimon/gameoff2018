using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform target;
    public bool position = true;
    public bool rotation = true;
    public bool scale = true;
    public Vector3 positionOffset = Vector3.zero;
    public Vector3 rotationOffset = Vector3.zero;
    public Vector3 scaleOffset = Vector3.zero;

    private void LateUpdate()
    {
        if( !target )
        {
            return;
        }

        if( position )
        {
            gameObject.transform.position = target.position + positionOffset;
        }

        if( rotation )
        {
            gameObject.transform.rotation = target.rotation * Quaternion.Euler(rotationOffset);
        }

        if( scale )
        {
            gameObject.transform.localScale = target.localScale + scaleOffset;
        }
    }
}