using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(MovementController))]
public class ShootController : MonoBehaviour
{
    public KeyCode shootKey = KeyCode.E;
    public Gun defaultGun;

    private Animator animator;
    private MovementController movementController;
    private List<GameObject> hierarchy = new List<GameObject>();
    private Gun equippedGun;

    private bool shooting = false;
    private float totalTime;
    private float elapsedTime;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        movementController = gameObject.GetComponent<MovementController>();

        // Get all the character hierarchy gameobjects.
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        for( int i = 0; i < children.Length; i++ )
        {
            Transform child = children[i];
            hierarchy.Add(child.gameObject);
        }

        // Add the default gun.
        AddGun(defaultGun);
    }

    private void Update()
    {
        CheckInput();

        if( !shooting )
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if( elapsedTime >= totalTime )
        {
            InstantiateEffect();
            InstantiateProjectile();
            elapsedTime = 0;
        }
    }

    private void CheckInput()
    {
        if( Input.GetKeyDown(shootKey) )
        {
            StartShoot();
        }

        if( Input.GetKeyUp(shootKey) )
        {
            EndShoot();
        }
    }

    private void Animate( Item item, SpriteRenderer renderer )
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        for ( int i = 0; i < item.spriteAnimationGroups.Count; i++ )
        {
            SpriteAnimationGroup group = item.spriteAnimationGroups[i];

            if( !state.IsName(group.name) )
            {
                continue;
            }

            for( int j = 0; j < group.spriteAnimations.Count; j++ )
            {
                SpriteAnimation animation = group.spriteAnimations[j];

                // Set position.
                renderer.transform.localPosition = animation.position;

                // Set rotation.
                renderer.transform.localRotation = Quaternion.Euler(animation.rotation);

                // Set scale.
                renderer.transform.localScale = animation.scale;

                // Set sprite.
                renderer.sprite = animation.sprite;

                // Set sorting order.
                if( !animation.ignoreOrder )
                {
                    renderer.sortingOrder = animation.sortingOrder;
                }
            }
        }
    }

    private void InstantiateEffect()
    {
        if( !equippedGun.shootEffect )
        {
            return;
        }

        GameObject go = Instantiate(equippedGun.shootEffect);
        ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();

        // Set gameobject parent.
        go.transform.SetParent(gameObject.transform);

        // Set gameobject position.
        go.transform.position = equippedGun.spriteRenderer.transform.position + movementController.faceDirection * equippedGun.effectOffset;

        // Set particle system rotation.
        if( movementController.faceDirection == Vector3.up )
        {
            ParticleSystem.MainModule module = particleSystem.main;
            module.startRotation = -90 * Mathf.Deg2Rad;
        }
        if( movementController.faceDirection == Vector3.right )
        {
            // Euler angles (0, 0, 0).
        }
        if( movementController.faceDirection == Vector3.down )
        {
            ParticleSystem.MainModule module = particleSystem.main;
            module.startRotation = 90 * Mathf.Deg2Rad;
        }
        if( movementController.faceDirection == Vector3.left )
        {
            ParticleSystem.MainModule module = particleSystem.main;
            module.startRotation = -180 * Mathf.Deg2Rad;
        }
    }

    private void InstantiateProjectile()
    {
        if( !equippedGun.projectile )
        {
            return;
        }

        GameObject go = Instantiate(equippedGun.projectile);
        Projectile projectile = go.GetComponent<Projectile>();
        projectile.damage = equippedGun.projectileDamage;
        projectile.speed = equippedGun.projectileSpeed;
        projectile.lifetime = equippedGun.projectileLifetime;
        projectile.destroyOnCollision = equippedGun.destroyOnCollision;
        projectile.owner = gameObject;

        // Set gameobject position.
        go.transform.position = equippedGun.spriteRenderer.transform.position + movementController.faceDirection * equippedGun.projectileOffset;

        // Set gameobject rotation.
        if( movementController.faceDirection == Vector3.up )
        {
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        if( movementController.faceDirection == Vector3.right )
        {
            // Euler angles (0, 0, 0).
        }
        if( movementController.faceDirection == Vector3.down )
        {
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        if( movementController.faceDirection == Vector3.left )
        {
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }

    public void OnAnimate()
    {
        Animate(equippedGun.item, equippedGun.spriteRenderer);
    }

    public void AddGun( Gun gun )
    {
        // If the gun is already equipped do nothing.
        if( equippedGun && equippedGun.name == gun.name )
        {
            return;
        }

        // Stop shooting and remove the gun.
        EndShoot();
        RemoveGun();

        // Instantiate the gun.
        equippedGun = ScriptableObject.Instantiate(gun);
        GameObject go = new GameObject(equippedGun.name);
        go.transform.localScale = Vector3.one;

        // Add the sprite renderer component.
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.material = equippedGun.item.material;
        renderer.sortingLayerName = equippedGun.item.sortingLayerName;
        equippedGun.spriteRenderer = renderer;

        // Set the gun parent.
        for( int i = 0; i < hierarchy.Count; i++ )
        {
            if( hierarchy[i].name == gun.item.gameObjectName )
            {
                go.transform.SetParent(hierarchy[i].transform);
                break;
            }
        }

        // Animate the new gun.
        OnAnimate();
    }

    public void RemoveGun()
    {
        if( !equippedGun )
        {
            return;
        }

        if( equippedGun.spriteRenderer )
        {
            Destroy(equippedGun.spriteRenderer.gameObject);
        }

        equippedGun = null;
    }

    public void StartShoot()
    {
        if( shooting )
        {
            return;
        }

        totalTime = 1f / equippedGun.projectileRate;
        elapsedTime = 0;
        shooting = true;
    }

    public void EndShoot()
    {
        shooting = false;
    }
}