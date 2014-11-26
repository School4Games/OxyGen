using UnityEngine;
using System.Collections;

public class RNG 
{

	public static int rollDice (int dice, int sides) 
	{
		int result = 0;
		for (int i=0; i<dice; i++) 
		{
			result += Mathf.RoundToInt(Random.value * sides);
		}
		return result;
	}

	public static int[] getEventCount (int dice, int sides) 
	{
		//int[] eventCount = new int[dice*sides];
		// ???? gaaaahrgh!!!!
		int[] eventCount = {0, 0, 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1};
		return eventCount;
	}
}