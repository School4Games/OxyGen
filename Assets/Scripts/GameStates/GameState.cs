using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{

	public MapState mapState;
	public FightState fightState;
	public DungeonState dungeonState;
	public LootState lootState;
	public LoseState loseState;
	public WinState winState;

	public WorldMap worldMap;

	public GameObject mapUI;
	public GameObject fightUI;
	public GameObject dungeonUI;
	public GameObject lootUI;
	public GameObject loseUI;
	public GameObject winUI;

	public Player player;

	public GameObject particlePrefab;

	public bool dungeoneering;
	int dungeonFloorsLeft; 
	public bool fighting;
	public bool looting;

	void Update () 
	{

	}

	// somehow counter intuitive flow ...
	public void chooseEvent (int terrain, int obj)
	{
		// is there an object on this tile?
		if (obj >= 0)
		{
			DungeonEvent[] dungeonEvents = worldMap.terrains[terrain].dungeonEvents;
			if (!dungeoneering)
			{
				// base
				if (obj == 0)
				{
					// test
					player.oxygen = 10;
				}
				// outpost
				if (obj == 1)
				{
					// test
					player.oxygen = 10;
				}
				// dungeons
				if (obj == 2)
				{
					// why did i make multiple dungeon events again?
					dungeonFloorsLeft = dungeonEvents[0].floors;
					dungeoneering = true;
				}
				// spaceship parts
				if (obj == 3)
				{
					player.parts++;
					// note to self: don't ever use vectors as int storage again
					worldMap.objects[(int)player.position.x, (int)player.position.y] = -1;
					// make parts diappear visually?
				}
			}
			// dungeon events
			// called from dungeon state
			else if (dungeonFloorsLeft > 0)
			{
				FightEvent[] dungeonFightEvents = dungeonEvents[0].fightEvents;
				foreach (FightEvent dungeonFightEvent in dungeonFightEvents) 
				{
					for (int i=0; i<=dungeonFightEvent.repetitions; i++)
					{
						if (Random.value < dungeonFightEvent.probability)
						{
							fightState.spawnEnemy(dungeonFightEvent.minDice, dungeonFightEvent.maxDice, dungeonFightEvent.minSides, dungeonFightEvent.maxSides, dungeonFightEvent.loot);
							fighting = true;
						}
					}
				}
				dungeonFloorsLeft--;
			}
			// loot
			else
			{
				ResourceEvent[] dungeonResourceEvents = dungeonEvents[0].resourceEvents;
				foreach (ResourceEvent dungeonResourceEvent in dungeonResourceEvents)
				{
					int amount = Random.Range (dungeonResourceEvent.minAmount, dungeonResourceEvent.maxAmount+1);
					if (Random.value < dungeonResourceEvent.probability)
					{
						if (dungeonResourceEvent.type == ResourceEvent.Type.Oxygen) 
						{
							player.oxygen += amount;
						}
						if (dungeonResourceEvent.type == ResourceEvent.Type.Water) 
						{
							player.water += amount;
						}
						if (dungeonResourceEvent.type == ResourceEvent.Type.Scrap) 
						{
							player.scrap += amount;
						}
						lootState.setLootText (amount, dungeonResourceEvent.type);
					}
				}
				dungeoneering = false;
				// active looting?
				looting = true;
			}
		}
		else
		{
			// fight events
			FightEvent[] fightEvents = worldMap.terrains[terrain].fightEvents;
			foreach (FightEvent fightEvent in fightEvents) 
			{
				for (int i=0; i<=fightEvent.repetitions; i++)
				{
					if (Random.value < fightEvent.probability)
					{
						fightState.spawnEnemy(fightEvent.minDice, fightEvent.maxDice, fightEvent.minSides, fightEvent.maxSides, fightEvent.loot);
						fighting = true;
					}
				}
			}

			// should pause here according to game design department

			// resource events
			ResourceEvent[] resourceEvents = worldMap.terrains[terrain].resourceEvents;
			foreach (ResourceEvent resourceEvent in resourceEvents)
			{
				int amount = Random.Range (resourceEvent.minAmount, resourceEvent.maxAmount+1);
				spawnParticles (amount);
				if (Random.value < resourceEvent.probability)
				{
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
		switchState ();
	}

	void spawnParticles (int amount)
	{
		GameObject particle = (GameObject) Instantiate (particlePrefab);
		particle.transform.position = player.transform.position;
		ResourceParticles resourceParticles = particle.GetComponent ("ResourceParticles") as ResourceParticles;
		resourceParticles.emit (amount);
	}

	// called from the currently active state to get to the other one
	// sort of a state machine now
	// got kind of gigantic. is probably stupid. 
	// something like state.SetActive(statebool) probably?
	public void switchState ()
	{
		// public var somwhere would be nice
		if (player.parts >= 4)
		{
			winUI.SetActive(true);
			loseUI.SetActive(false);
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		// deaths
		else if (player.oxygen <= 0)
		{
			loseUI.SetActive(true);
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		else if (player.water <= 0)
		{
			loseUI.SetActive(true);
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		else if (player.health <= 0)
		{
			loseUI.SetActive(true);
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		else if (fighting)
		{
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(true);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(true);
			dungeonState.gameObject.SetActive (false);
		}
		else if (dungeoneering)
		{
			lootState.gameObject.SetActive(false);
			dungeonUI.SetActive(true);
			fightUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (true);
		}
		else if (looting)
		{
			lootState.gameObject.SetActive(true);
			lootUI.SetActive(true);
			dungeonUI.SetActive(false);
			fightUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (true);
		}
		else
		{
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(true);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
	}
}
