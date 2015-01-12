using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{

	public MapState mapState;
	public FightState fightState;
	public DungeonState dungeonState;

	public WorldMap worldMap;

	public GameObject mapUI;
	public GameObject fightUI;

	public Player player;

	public enum State{Map, Fight, Dungeon};

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
	public void switchState (State state)
	{
		if (state = State.Dungeon)
		{
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (true);
		}
		if (state = State.Fight)
		{
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (true);
		}
		if (state = State.Map)
		{
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (true);
		}
		mapState.gameObject.SetActive(!mapState.gameObject.activeSelf);
		fightUI.gameObject.SetActive(!fightState.gameObject.activeSelf);
		fightState.gameObject.SetActive(!fightState.gameObject.activeSelf);
	}
}
