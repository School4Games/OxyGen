using UnityEngine;
using System.Collections;

[System.Serializable]
public class FightEvent : Event {

	public int minDice;
	public int maxDice;

	public int minSides;
	public int maxSides;

	// normal loot is half the maximum attack value
	public int lootBonus;
}
