using UnityEngine;

public class AnimateSprites : StateMachineBehaviour
{
    public SpriteAnimator spriteAnimator;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
        if( spriteAnimator )
        {
            spriteAnimator.Animate();
        }
	}
}