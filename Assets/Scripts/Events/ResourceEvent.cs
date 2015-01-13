using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResourceEvent : Event {

	public enum Type {Water, Oxygen, Scrap};
	public Type type;

	public int minAmount;
	public int maxAmount;
}
