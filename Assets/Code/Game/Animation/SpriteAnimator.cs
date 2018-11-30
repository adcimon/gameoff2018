using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SpriteAnimator : MonoBehaviour
{
    public List<SpriteAnimationGroup> spriteAnimationGroups = new List<SpriteAnimationGroup>();
    [Space(10)] public UnityEvent onAnimate;

    private Animator animator;
    private Dictionary<string, SpriteRenderer> spriteRenderers = new Dictionary<string, SpriteRenderer>();

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        // Get the animate sprites state machine behaviours and initialize them.
        AnimateSprites[] animateSprites = animator.GetBehaviours<AnimateSprites>();
        for( int i = 0; i < animateSprites.Length; i++ )
        {
            animateSprites[i].spriteAnimator = this;
        }

        // Get all the sprite renderers to animate.
        SpriteRenderer[] renderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
        for( int i = 0; i < renderers.Length; i++ )
        {
            SpriteRenderer renderer = renderers[i];
            spriteRenderers.Add(renderer.gameObject.name, renderer);
        }
    }

    public void Animate()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        for( int i = 0; i < spriteAnimationGroups.Count; i++ )
        {
            SpriteAnimationGroup group = spriteAnimationGroups[i];

            if( !state.IsName(group.name) )
            {
                continue;
            }

            // Set character rotation.
            gameObject.transform.localRotation = Quaternion.Euler(group.eulerAngles);

            for( int j = 0; j < group.spriteAnimations.Count; j++ )
            {
                SpriteAnimation animation = group.spriteAnimations[j];

                if( spriteRenderers.ContainsKey(animation.gameObjectName) )
                {
                    SpriteRenderer renderer = spriteRenderers[animation.gameObjectName];

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

        onAnimate.Invoke();
    }
}