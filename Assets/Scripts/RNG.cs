using UnityEngine;
using System.Collections;

public class RNG 
{

	public static int rollDice (int dice, int sides) 
	{
		int result = 0;
		for (int i=0; i<dice; i++) 
		{
			result += Random.Range(1, sides+1);
		}
		return result;
	}

	// i hope those words mean what i'm trying to say. way too long and stupid sounding anyway ...
	public static int getMaximumAbsoluteProbability (int dice, int sides) 
	{
		int maximum = 0;
		// again stupid approach ftw ...
		foreach (int eventCount in getEventCount(dice, sides)) 
		{
			if (eventCount > maximum)
			{
				maximum = eventCount;
			}
		}
		return maximum;
	}

	// performance critical method (on spawn enemy in fights)
	// as always: don't do weird stuff here or at least save all progress before you do
	// optimize: Lars (recursive dice-reducing-until-1-die + "Memoize")
	public static int[] getEventCount (int dice, int sides) 
	{
		int[] eventCount = new int[dice*sides+1];
		// ???? gaaaahrgh!!!!
		// stupid approach: int[] eventCount = {0, 0, 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1};
		// slightly less stupid approach. Because it's a computer, and computers are proficient in doing stupid stuff, right?
		// count ways to combine dice for all events. yes, all of them. ONE BY ONE.
		eventCount = loop (sides, dice, eventCount, 0);
		return eventCount;
	}
	private static int[] loop (int sides, int diceLeft, int[] eventCount, int sum)
	{
		for (int dx=1; dx<=sides; dx++) 
		{
			// open anoter loop
			if (diceLeft > 1) 
			{
				loop (sides, diceLeft-1, eventCount, sum + dx);
			}
			// stop looping, start adding
			else
			{
				int e = sum + dx;
				eventCount[e]++;
			}
		}
		return eventCount;
	}
}