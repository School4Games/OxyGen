using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Resource
{

	[HideInInspector]
	public string name;

	public int amount;
	public int stackSize;
	public Color color;
	public Sprite icon;

	public enum Type 
	{
		Water, 
		Oxygen, 
		Scrap, 
		Part, 
		Health, 
		Armor,

		Length
	};
	[HideInInspector]
	public Type type;

	public override string ToString ()
	{
		return type.ToString();
	}
}
