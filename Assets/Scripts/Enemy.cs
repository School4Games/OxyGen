using UnityEngine;
using System.Collections;

public class Enemy {

	public int attackCount = 2;

	public int dice = 2;

	public int sides = 6;

	// as water
	public int loot = 6;

	public int attack () 
	{
		return RNG.rollDice (dice, sides);
	}

	/*void OnTurn () 
	 * {
		Debug.Log ("");
	}*/
}
