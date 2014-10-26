using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WorldMap))]
public class WorldMapEditor : Editor 
{
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		WorldMap map = (WorldMap)target;
		if (GUILayout.Button("Generate")) {
			map.generate();
		}
	}
}
