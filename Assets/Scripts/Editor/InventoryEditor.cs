using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor 
{
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		Inventory inventory = (Inventory)target;
		Array.Resize(ref inventory.resources, (int)Resource.Type.Length);
		for (int i=0; i<(int)Resource.Type.Length; i++) 
		{
			inventory.resources[i].type = (Resource.Type)i;
			inventory.resources[i].name = inventory.resources[i].type.ToString();
		}
	}
}
