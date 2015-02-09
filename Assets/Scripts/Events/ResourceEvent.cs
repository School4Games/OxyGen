using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResourceEvent : Event {
	
	public Resource.Type type;

	public int minAmount;
	public int maxAmount;
}
