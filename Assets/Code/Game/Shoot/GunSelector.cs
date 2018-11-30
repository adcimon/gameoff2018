using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GunSelector : MonoBehaviour
{
    public Gun gun;

    private void OnTriggerEnter2D( Collider2D other )
    {
        ShootController shootController = other.gameObject.GetComponent<ShootController>();
        if( !shootController )
        {
            return;
        }

        if( !gun )
        {
            return;
        }

        shootController.AddGun(gun);
    }
}