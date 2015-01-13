using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{

	public MapState mapState;
	public WorldMap worldMap;
	
	public FightState fightState;

	public Player player;

	public GameObject particlePrefab;

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
					int amount = Random.Range (resourceEvent.minAmount, resourceEvent.maxAmount+1);
					spawnParticles (amount);
					if (resourceEvent.type == ResourceEvent.Type.Oxygen) 
					{
						player.oxygen += amount;
					}
					if (resourceEvent.type == ResourceEvent.Type.Water) 
					{
						player.water += amount;
					}
					if (resourceEvent.type == ResourceEvent.Type.Scrap) 
					{
						player.scrap += amount;
					}
				}
			}
		}
	}

	void spawnParticles (int amount)
	{
		GameObject particle = (GameObject) Instantiate (particlePrefab);
		particle.transform.position = player.transform.position;
		ResourceParticles resourceParticles = particle.GetComponent ("ResourceParticles") as ResourceParticles;
		resourceParticles.emit (amount);
	}

	// called from the currently active state to get to the other one
	public void switchState ()
	{
		mapState.gameObject.SetActive(!mapState.gameObject.activeSelf);
		fightState.gameObject.SetActive(!fightState.gameObject.activeSelf);
	}
}
