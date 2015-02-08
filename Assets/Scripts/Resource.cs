using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource {

	[HideInInspector]
	public string name;

	public int amount;
	public int stackSize;
	public Color color;

	public enum Type 
	{
		Water, 
		Oxygen, 
		Scrap, 
		Part, 
		Health, 

		Length
	};
	[HideInInspector]
	public Type type;

	public override string ToString ()
	{
		return type.ToString();
	}
}
