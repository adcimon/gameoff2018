using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor
{
    private GameManager script;

	private void Awake()
	{
		script = (GameManager)target;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
        GUILayout.Space(10);
        if( GUILayout.Button("End Game") )
        {
            script.EndGame();
        }
	}
}