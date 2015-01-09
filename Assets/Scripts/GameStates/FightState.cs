using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightState : MonoBehaviour, IFightMenuMessageTarget
{

	public Player player;

	public GameState gamestate;

	ArrayList enemies = new ArrayList();

	int shield = 0;

	public int dmg = -1;

	// "lootboost"
	// how do we call this?
	int pot = 0;

	// dmg pointer
	public int dmgX = 0;
	public int dmgY = 0;
	// not actually time per cycle anymore
	public float maxTimePerCycle = 5;
	public int cycles = 2;

	bool rolling = false;

	int roundsSurvived = 0;

	// UI
	public GameObject graphWindow;
	Rect graphRect;
	float maxProbability = 0;
	float maxDmg = 0;
	int[] eventCount;
	public Slider shieldSlider;
	public Text statsDisplay;
	public Image pointer;

	public GameObject barPrefab;

	// Use this for initialization
	void Start () 
	{
		pointer.enabled = false;
		// test
		/*foreach (int eventCount in RNG.getEventCount(2, 6))
		{
			log += eventCount + ", ";
		}*/
		//Debug.Log(RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides));
		//Debug.Log(log);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// all enemies defeated -> back to map
		if (enemies.Count == 0 && !rolling) {
			gamestate.switchState ();
		}
		else 
		{
			updateStatsDisplay ();
		}
	}

	void OnEnable ()
	{
		// update slider dimensions
		updateSliderDimensions ();
		// update graph
		drawGraph ();
	}

	public void OnRoll ()
	{
		StartCoroutine ("roll");
	}

	void updateSliderDimensions ()
	{
		// can bet more than is currently in possession
		float maxDmg = (enemies[0] as Enemy).dice * (enemies[0] as Enemy).sides;
		shieldSlider.maxValue = maxDmg;
	}

	void updateStatsDisplay ()
	{
		shield = Mathf.RoundToInt(shieldSlider.value);
		statsDisplay.text = "";
		statsDisplay.text += "Health: " + player.health + "\n";
		statsDisplay.text += "Shields: " + shield + "\n";
		//statsDisplay.text += "Lootboost: " + pot+ "\n";
		string winLoot = "<color=#00FF00>" + ((enemies[0] as Enemy).loot - shield + pot*2) + "</color>";
		string looseLoot = "<color=#FF0000>" + (- shield) + "</color>";
		// water
		statsDisplay.text += "Water: " + (player.water /*- shield - pot*/) + " (" + winLoot + "/" + looseLoot + ")" + "\n";
	}

	void updatePointerPosition () {
		RectTransform pointerRect = pointer.GetComponent<RectTransform> ();

		float height = (float)dmgY/maxProbability;

		// set anchors to right percentage
		pointerRect.anchorMin = new Vector2 ((dmgX-1)/maxDmg, height);
		pointerRect.anchorMax = new Vector2 ((dmgX)/maxDmg, height + 0.05f);

		// make graphics fit anchors
		pointerRect.sizeDelta = Vector2.one;
	}

	void drawGraph () 
	{
		// delete old graph
		foreach (Image panel in graphWindow.GetComponentsInChildren<Image>())
		{
			if (panel.gameObject.name.Contains("Bar"))
			{
				Destroy(panel.gameObject);
			}
		}

		graphRect = graphWindow.GetComponent<RectTransform>().rect;
		maxDmg = (enemies[0] as Enemy).dice * (enemies[0] as Enemy).sides;
		maxProbability = RNG.getMaximumAbsoluteProbability((enemies[0] as Enemy).dice, (enemies[0] as Enemy).sides);
		eventCount = RNG.getEventCount((enemies[0] as Enemy).dice, (enemies[0] as Enemy).sides);
		
		// dmg
		for (int x=1; x<=maxDmg; x++) 
		{
			if (eventCount[x]>0) 
			{
			GameObject bar = (GameObject) Instantiate(barPrefab, Vector3.zero, Quaternion.identity);
			RectTransform barRect = bar.GetComponent<RectTransform>();

			float height = (float)eventCount[x]/maxProbability;
			// set anchors to right percentage 
			barRect.anchorMin = new Vector2 ((x-1)/maxDmg, 0);
			barRect.anchorMax = new Vector2 ((x)/maxDmg, height);
			
			// make graphics fit anchors
			barRect.sizeDelta = Vector2.one;

			bar.transform.SetParent(graphWindow.transform, false);
			// transform seems to be in relation to bottom center ...

			// test; lower left corner
			//barRect.anchorMin = Vector2.zero;
			//barRect.anchorMax = Vector2.one;

			// GUI.Label (new Rect(graphWindow.x + (x-1)*(graphWindow.width/maxDmg), graphWindow.y + y*(graphWindow.height/RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides)), graphWindow.width/maxDmg, graphWindow.height/RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides)), ""+(x), style);
		
			}
		}
	}

	public void spawnEnemy (int minDice, int maxDice, int minSides, int maxSides, int loot) 
	{
		Enemy newEnemy = new Enemy();
		newEnemy.dice = Random.Range (minDice, maxDice+1);;
		newEnemy.sides = Random.Range (minSides, maxSides+1);;
		newEnemy.loot = loot;

		enemies.Add (newEnemy);

		// if the spawned enemy is the only enemy in the array
		if (enemies.Count == 1)
		{
			drawGraph ();
		}
	}

	IEnumerator roll () 
	{
		rolling = true;
		int maxDmg = (enemies[0] as Enemy).dice * (enemies[0] as Enemy).sides;
		// do roll
		dmg = (enemies[0] as Enemy).attack ();

		int[] eventCount = RNG.getEventCount((enemies[0] as Enemy).dice, (enemies[0] as Enemy).sides);
		// cycle
		int fieldCount = 0;
		// add eventCount values
		foreach (int e in eventCount) 
		{
			fieldCount += e;
		}
		float timePerField = Mathf.Min (maxTimePerCycle / fieldCount, 0.1f);
		int stopHeight = Random.Range(0, eventCount[dmg]-1);
		int cyclesLeft = cycles-1;
		// ding, ding, ding, ding, ding
		while (true) {
			if (dmgX == dmg && dmgY == stopHeight && cyclesLeft <= 0) 
			{
				break;
			}
			if (dmgX < (enemies[0] as Enemy).dice) 
			{
				dmgX = (enemies[0] as Enemy).dice-1;
			}
			if (dmgY < 0)
			{
				dmgY = 0;
			}
			if (dmgY < eventCount[dmgX]-1) {
				dmgY++;
				yield return new WaitForSeconds (timePerField*(2/Mathf.Pow((cyclesLeft+1), 2)));
			}
			else 
			{
				dmgY = 0;
				if (dmgX < maxDmg)
				{
					dmgX++;
				}
				else 
				{
					dmgX = (enemies[0] as Enemy).dice;
					cyclesLeft--;
				}
				yield return new WaitForSeconds (timePerField*Mathf.Pow((2/(cyclesLeft+1)), 2));
			}
			updatePointerPosition ();
			pointer.enabled = true;
		}
		yield return new WaitForSeconds (1.0f);
		// confiscate stakes
		player.water -= shield;
		player.water -= pot;
		// do damage if attack hits
		if (dmg > shield) 
		{
			player.health -= 1;
		}
		// otherwise give loot
		else 
		{
			player.water += pot*2;
			player.water += (enemies[0] as Enemy).loot;
		}
		// reset shield and pot (and rolling)
		shieldSlider.value = 0;
		shield = 0;
		pot = 0;
		// reset pointer
		dmgX = 0;
		dmgY = 0;
		updatePointerPosition ();
		pointer.enabled = false;
		// kill enemy
		enemies.RemoveAt (0);
		rolling = false;
	}
}
