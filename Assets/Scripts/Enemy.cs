using UnityEngine;
using System.Collections;

public class Enemy {

	public int dice = 2;

	public int sides = 6;

	// as water
	public int loot = 6;

	// TODO: find better names for parameters
	public void init (int d, int s, int lootBonus)
	{
		dice = d;
		sides = s;
		loot = (d*s)/2 + lootBonus;
	}

	public int attack () 
	{
		return RNG.rollDice (dice, sides);
	}
}
