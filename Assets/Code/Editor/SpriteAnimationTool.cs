using UnityEngine;
using UnityEditor;

public class SpriteAnimationTool : EditorWindow
{
    private new string name = "";
    private GameObject model;

	[MenuItem("Window/SpriteAnimationTool")]
	private static void Init()
	{
		SpriteAnimationTool window = (SpriteAnimationTool)EditorWindow.GetWindow(typeof(SpriteAnimationTool));
		window.Show();
	}

	private void OnGUI()
	{
        GUILayout.Space(10);
        name = EditorGUILayout.TextField("Name", name);
        GUILayout.Space(10);
        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), true);
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("Make sure the gameobjects and sprite renderers are enabled.", MessageType.Warning);
        GUILayout.Space(10);
        if( GUILayout.Button("Create") )
        {
            Create();
        }
	}

    private void Create()
    {
        if( name == "" || !model )
        {
            return;
        }

        // Instantiate the animation group.
        SpriteAnimationGroup group = ScriptableObject.CreateInstance<SpriteAnimationGroup>();
        group.name = name;

        // Traverse the gameobject renderers to create the sprite animations.
        SpriteRenderer[] renderers = model.GetComponentsInChildren<SpriteRenderer>();
        for( int i = 0; i < renderers.Length; i++ )
        {
            SpriteRenderer renderer = renderers[i];
            if( !renderer.enabled || !renderer.gameObject.activeInHierarchy )
            {
                continue;
            }

            // Instantiate the sprite animation.
            SpriteAnimation animation = new SpriteAnimation();
            animation.gameObjectName = renderer.gameObject.name;
            animation.position = renderer.transform.localPosition;
            animation.rotation = renderer.transform.localRotation.eulerAngles;
            animation.scale = renderer.transform.localScale;
            animation.sprite = renderer.sprite;
            animation.sortingOrder = renderer.sortingOrder;
            animation.ignoreOrder = false;

            // Add the sprite animation to the sprite animation group.
            group.spriteAnimations.Add(animation);
        }

        // Save the sprite animation group.
        AssetDatabase.CreateAsset(group, "Assets/" + name + ".asset");
    }
}