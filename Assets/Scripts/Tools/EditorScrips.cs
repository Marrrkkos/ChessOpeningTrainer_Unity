using UnityEngine;
using UnityEditor;
using System.Collections;

class EditorScrips : EditorWindow
{

	[MenuItem("Play/PlayMe _%h")]
	public static void RunMainScene()
	{
    	EditorApplication.OpenScene("Assets/Scenes/RootScene.unity");
		EditorApplication.isPlaying = true;
	}
}