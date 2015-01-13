using UnityEngine;
using System.Collections;

[System.Serializable]
public class DungeonEvent : Event {

	public ResourceEvent[] resourceEvents;
	public FightEvent[] fightEvents;

	public int floors;
}
