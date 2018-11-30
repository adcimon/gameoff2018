using UnityEngine;

public class CameraTransitionEffect : MonoBehaviour
{
	public Material transitionMaterial;

	public Texture transitionTexture;

	public Color color = Color.black;

	[Range(0, 1)]
	public float cutoff = 0;

	[Range(0, 1)]
	public float fade = 1;

    private bool playing = false;
    private int inOut = 1;
    private float duration;
    private float elapsedTime = 0;

    private void Update()
    {
        if( !playing )
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        cutoff = (inOut == 1) ? (elapsedTime / duration) : (1 - (elapsedTime / duration));
        if( elapsedTime >= duration )
        {
            cutoff = (inOut == 1) ? 1 : 0;
            playing = false;
        }
    }

    private void OnRenderImage( RenderTexture src, RenderTexture dst )
	{
        if( transitionMaterial == null || transitionTexture == null )
        {
            return;
        }

		transitionMaterial.SetTexture("_TransitionTex", transitionTexture);
		transitionMaterial.SetColor("_Color", color);
		transitionMaterial.SetFloat("_Cutoff", cutoff);
		transitionMaterial.SetFloat("_Fade", fade);
		Graphics.Blit(src, dst, transitionMaterial);
	}

    public void Play( float duration )
    {
        if( playing )
        {
            return;
        }

        if( cutoff == 0 )
        {
            inOut = 1;
        }
        if( cutoff == 1 )
        {
            inOut = -1;
        }

        this.duration = duration;
        elapsedTime = 0;
        playing = true;
    }
}