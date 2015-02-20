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
	public TutorialState tutorialState;

	public Animator overlayAnimator;

	AudioSource loopAudioSource;
	AudioSource effectAudioSource;
	public AudioClip[] stepSounds;
	public AudioClip oxygenRefillSound;
	public AudioClip getWaterSound;
	public AudioClip dieSound;
	public AudioClip enemyDieSound;
	public AudioClip areaClearSound;
	public AudioClip enemyWinSound;
	public AudioClip battleLoop;
	public AudioClip mapLoop;
	public AudioClip winLoop;
	// hehe
	public AudioClip waitLoop;

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

	void Start () 
	{
		loopAudioSource = Camera.main.GetComponents<AudioSource> ()[0];
		effectAudioSource = Camera.main.GetComponents<AudioSource> ()[1];
		tutorialState.enableMessage (0);
	}

	public void playLoop (AudioClip sound)
	{
		if (loopAudioSource.clip != sound)
		{
			loopAudioSource.clip = sound;
			loopAudioSource.Play ();
		}
	}

	public void playSound (AudioClip sound)
	{
		effectAudioSource.clip = sound;
		effectAudioSource.PlayOneShot (sound);
	}

	public void playRandomSound (AudioClip[] sounds)
	{
		int index = Random.Range (0, sounds.Length);
		effectAudioSource.PlayOneShot (sounds[index]);
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
					outposting = true;
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
					worldMap.objectGraphics[(int)player.position.x, (int)player.position.y].color = Color.gray;
				}
				// spaceship parts
				if (obj == 3)
				{
					player.inventory.addResource (Resource.Type.Part, 1);
					// note to self: don't ever use vectors as int storage again
					worldMap.objects[(int)player.position.x, (int)player.position.y] = -1;
					worldMap.objectGraphics[(int)player.position.x, (int)player.position.y].color = Color.clear;
					// make parts disappear visually?
				}
			}
			// dungeon events
			// called from dungeon state
			else if (dungeonFloorsLeft > 0)
			{
				Debug.Log ("chosing dungeon event ...");
				FightEvent[] dungeonFightEvents = dungeonEvents[0].fightEvents;
				foreach (FightEvent dungeonFightEvent in dungeonFightEvents) 
				{
					for (int i=0; i<=dungeonFightEvent.repetitions; i++)
					{
						if (Random.value < dungeonFightEvent.probability)
						{
							Debug.Log ("dungeon fight event!" + terrain);
							fightUI.GetComponent<Image>().overrideSprite = worldMap.terrains[terrain].dungeonScreen;
							enemyGraphics.overrideSprite = worldMap.terrains[terrain].dungeonEnemy;
							fightState.spawnEnemy(dungeonFightEvent.minDice, dungeonFightEvent.maxDice, dungeonFightEvent.minSides, dungeonFightEvent.maxSides, dungeonFightEvent.lootBonus);
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
						fightState.spawnEnemy(fightEvent.minDice, fightEvent.maxDice, fightEvent.minSides, fightEvent.maxSides, fightEvent.lootBonus);
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
				// just water sound for now
				playSound (getWaterSound);
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
			playLoop (winLoop);
			loopAudioSource.loop = false;
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
		// deaths(/ taking damage)
		else if (player.inventory.getResources()[(int)Resource.Type.Oxygen].amount <= 0 && (worldMap.objects[(int)player.position.x, (int)player.position.x] < 0 || worldMap.objects[(int)player.position.x, (int)player.position.x] > 1))
		{
			// make player get hit sound instead?
			playSound (enemyWinSound);
			player.inventory.addResource (Resource.Type.Health, -1);
			overlayAnimator.Play ("fightHit");
			if (player.inventory.getResources()[(int)Resource.Type.Health].amount <= 0) loseText.text = loseText.text.Replace ("died", "suffocated");
		}
		else if (player.inventory.getResources()[(int)Resource.Type.Water].amount <= 0)
		{
			playSound (enemyWinSound);
			player.inventory.addResource (Resource.Type.Health, -1);
			overlayAnimator.Play ("fightHit");
			if (player.inventory.getResources()[(int)Resource.Type.Health].amount <= 0) loseText.text = loseText.text.Replace ("died", "dehydrated");
		}
		// actually die here
		if (player.inventory.getResources()[(int)Resource.Type.Health].amount <= 0)
		{
			overlayAnimator.Play ("fightHit");
			playSound (dieSound);
			loopAudioSource.Stop ();
			loseUI.SetActive(true);
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
			playLoop (battleLoop);
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
			playLoop (mapLoop);
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
