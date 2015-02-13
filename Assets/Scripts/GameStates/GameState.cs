using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameState : MonoBehaviour 
{
	// hmmm ...
	public static Resource draggedResource;

	public MapState mapState;
	public FightState fightState;
	public DungeonState dungeonState;
	public LootState lootState;
	public LoseState loseState;
	public WinState winState;
	public OutpostState outpostState;

	public WorldMap worldMap;

	public GameObject fightUI;
	public Image enemyGraphics;
	public GameObject dungeonUI;
	public GameObject lootUI;
	public GameObject loseUI;
	public Text loseText;
	public GameObject winUI;
	public GameObject inventoryUI;
	public GameObject outpostUI;

	public Player player;

	public GameObject particlePrefab;

	public bool dungeoneering;
	public bool outposting;
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

				}
				// outpost
				if (obj == 1)
				{
					outposting = true;
				}
				// dungeons
				if (obj == 2)
				{
					// why did i make multiple dungeon events again?
					dungeonFloorsLeft = dungeonEvents[0].floors;
					dungeonUI.GetComponent<Image>().overrideSprite = worldMap.terrains[terrain].dungeonScreen;
					dungeoneering = true;
					worldMap.objects[(int)player.position.x, (int)player.position.y] = -1;
				}
				// spaceship parts
				if (obj == 3)
				{
					player.inventory.addResource (Resource.Type.Part, 1);
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
							fightUI.GetComponent<Image>().overrideSprite = worldMap.terrains[terrain].dungeonScreen;
							enemyGraphics.overrideSprite = worldMap.terrains[terrain].dungeonEnemy;
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
						lootState.lootInventory.addResource (dungeonResourceEvent.type, amount);
					}
				}
				dungeoneering = false;
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
						fightUI.GetComponent<Image>().overrideSprite = worldMap.terrains[terrain].outdoorScreen;
						enemyGraphics.overrideSprite = worldMap.terrains[terrain].outdoorEnemy;
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
					player.inventory.addResource (resourceEvent.type, amount);
				}
			}
		}
		player.inventory.updateSlotVisuals ();
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
		if (player.inventory.getResources()[(int)Resource.Type.Part].amount >= 4)
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
		else if (player.inventory.getResources()[(int)Resource.Type.Oxygen].amount <= 0)
		{
			loseUI.SetActive(true);
			loseText.text = "You suffocated.\nPress R to restart";
			loseState.gameObject.SetActive (true);
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		else if (player.inventory.getResources()[(int)Resource.Type.Water].amount <= 0)
		{
			loseUI.SetActive(true);
			loseText.text = "You dehydrated.\nPress R to restart";
			loseState.gameObject.SetActive (true);
			lootState.gameObject.SetActive(false);
			fightUI.SetActive(false);
			dungeonUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		else if (player.inventory.getResources()[(int)Resource.Type.Health].amount <= 0)
		{
			loseUI.SetActive(true);
			loseText.text = "You died.\nPress R to restart";
			loseState.gameObject.SetActive (true);
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
			outpostState.gameObject.SetActive(false);
			outpostUI.SetActive(false);
			inventoryUI.SetActive (false);
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
			outpostState.gameObject.SetActive(false);
			outpostUI.SetActive(false);
			inventoryUI.SetActive (false);
			lootState.gameObject.SetActive(false);
			dungeonUI.SetActive(true);
			fightUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (true);
		}
		else if (outposting)
		{
			outpostState.gameObject.SetActive(true);
			outpostUI.SetActive(true);
			inventoryUI.SetActive (true);
			lootState.gameObject.SetActive(false);
			dungeonUI.SetActive(false);
			fightUI.SetActive(false);
			lootUI.SetActive(false);
			mapState.gameObject.SetActive(false);
			fightState.gameObject.SetActive(false);
			dungeonState.gameObject.SetActive (false);
		}
		else if (looting)
		{
			outpostState.gameObject.SetActive(false);
			outpostUI.SetActive(false);
			inventoryUI.SetActive (true);
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
			outpostState.gameObject.SetActive(false);
			outpostUI.SetActive(false);
			inventoryUI.SetActive (true);
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
