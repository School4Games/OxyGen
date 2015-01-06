using UnityEngine;
using System.Collections;

[System.Serializable]
public class Terrain {

	public string name;

	public Texture2D texture;

	public ResourceEvent[] resourceEvents;
	public FightEvent[] fightEvents;
}
