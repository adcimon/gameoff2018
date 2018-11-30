using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Destructible))]
public class DestructibleInspector : Editor
{
    private Destructible script;

	private void Awake()
	{
		script = (Destructible)target;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
        if( GUILayout.Button("Impact") )
        {
            script.Impact(script.transform.position, Vector3.right);
        }
	}
}