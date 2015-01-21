using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource {

	public int amount;
	public int stackSize;
	public Color color;

	public override string ToString ()
	{
		return amount.ToString ();
	}
}
