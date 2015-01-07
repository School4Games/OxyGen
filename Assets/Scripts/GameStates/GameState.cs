using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{

	public MapState mapState;
	public WorldMap worldMap;
	
	public FightState fightState;

	public Player player;

	void Update () 
	{
		// test
		if (Input.GetKeyDown(KeyCode.M))
		{
			switchState ();
		}
	}

	public void chooseEvent (int terrain, int obj)
	{
		// is there an object on this tile?
		if (obj >= 0)
		{

		}
		else
		{
			// fight events
			FightEvent[] fightEvents = worldMap.terrains[terrain].fightEvents;
			bool enemySpawned = false;
			foreach (FightEvent fightEvent in fightEvents) 
			{
				for (int i=0; i<=fightEvent.repetitions; i++)
				{
					if (Random.value < fightEvent.probability)
					{
						fightState.spawnEnemy(fightEvent.minDice, fightEvent.maxDice, fightEvent.minSides, fightEvent.maxSides, fightEvent.loot);
						enemySpawned = true;
					}
				}
			}
			if (enemySpawned) 
			{
				switchState ();
			}

			// should pause here according to game design department

			// resource events
			ResourceEvent[] resourceEvents = worldMap.terrains[terrain].resourceEvents;
			foreach (ResourceEvent resourceEvent in resourceEvents)
			{
				if (Random.value < resourceEvent.probability)
				{
					if (resourceEvent.type == ResourceEvent.Type.Oxygen) 
					{
						player.oxygen += Random.Range (resourceEvent.minAmount, resourceEvent.maxAmount+1);
					}
					if (resourceEvent.type == ResourceEvent.Type.Water) 
					{
						player.water += Random.Range (resourceEvent.minAmount, resourceEvent.maxAmount+1);
					}
					if (resourceEvent.type == ResourceEvent.Type.Scrap) 
					{
						player.scrap += Random.Range (resourceEvent.minAmount, resourceEvent.maxAmount+1);
					}
				}
			}
		}
	}

	// called from the currently active state to get to the other one
	public void switchState ()
	{
		mapState.gameObject.SetActive(!mapState.gameObject.activeSelf);
		fightState.gameObject.SetActive(!fightState.gameObject.activeSelf);
	}
}
