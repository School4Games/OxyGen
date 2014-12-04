using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightState : MonoBehaviour, IFightMenuMessageTarget
{

	Player player;

	Enemy enemy;

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

	// test
	public int enemyDice = 3;
	public int enemySides = 5;

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
		// test
		// get actual player here instead of creating new one
		player = new Player ();
		//string log = "";
		spawnEnemy ();
		// test
		drawGraph ();

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
		updateStatsDisplay ();
	}

	public void OnRoll ()
	{
		StartCoroutine ("roll");
	}

	void updateSliderDimensions ()
	{
		// can bet more than is currently in possession
		float maxDmg = enemy.dice * enemy.sides;
		shieldSlider.maxValue = maxDmg;
	}

	void updateStatsDisplay ()
	{
		shield = Mathf.RoundToInt(shieldSlider.value);
		statsDisplay.text = "";
		statsDisplay.text += "Health: " + player.health + "\n";
		statsDisplay.text += "Shields: " + shield + "\n";
		//statsDisplay.text += "Lootboost: " + pot+ "\n";
		string winLoot = "<color=#00FF00>" + (enemy.loot - shield + pot*2) + "</color>";
		string looseLoot = "<color=#FF0000>" + (- shield) + "</color>";
		// water
		statsDisplay.text += "Scrap: " + (player.water /*- shield - pot*/) + " (" + winLoot + "/" + looseLoot + ")" + "\n";
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
		maxDmg = enemy.dice * enemy.sides;
		maxProbability = RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides);
		eventCount = RNG.getEventCount(enemy.dice, enemy.sides);
		
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

			// test; lower lesft corner
			//barRect.anchorMin = Vector2.zero;
			//barRect.anchorMax = Vector2.one;

			// GUI.Label (new Rect(graphWindow.x + (x-1)*(graphWindow.width/maxDmg), graphWindow.y + y*(graphWindow.height/RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides)), graphWindow.width/maxDmg, graphWindow.height/RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides)), ""+(x), style);
		
			}
		}
	}

	/*void OnGUI () 
	{
		int maxDmg = enemy.dice * enemy.sides;
		//player stats
		GUI.Label (new Rect(10, 10, 120, 20), "Health: " + player.health);
		GUI.Label (new Rect(10, 40, 120, 20), "Water: " + (player.water - shield - pot));

		GUI.Label (new Rect(140, 10, 120, 20), "Shields: " + shield);
		// bet on shields
		shield = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect(270, 10, 120, 20), shield, 0, maxDmg - pot));

		GUI.Label (new Rect(140, 40, 120, 20), "Bet: " + pot);
		// bet on pot
		pot = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect(270, 40, 120, 20), pot, 0, Mathf.Min(player.water - shield, maxDmg - shield)));
		
		// stop betting, start rolling
		if (!rolling) {
			if (GUI.Button (new Rect(400, 10, 120, 50), "Hit me!"))
			{
				// reset pointer
				dmgX = 0;
				dmgY = 0;
				StartCoroutine ("roll");
				if (player.health > 0) {
					roundsSurvived++;
				}
			}
		}
		
		// rules for loot
		// win case
		GUI.Label (new Rect(530, 10, 150, 50), "Win: gain " + (pot + enemy.loot - shield) + " water");
		// lose case
		GUI.Label (new Rect(530, 40, 150, 50), "Lose: gain " + (-pot - shield) + " water");
		
		// for testing purpose only
		GUI.Label (new Rect(Screen.width - 160, 10, 150, 50), "You survived for " + roundsSurvived + " rounds");

		// background (graph window)
		GUI.Box (new Rect(graphWindow.x -10, graphWindow.y -10, graphWindow.width + 20, graphWindow.height +20), "");
		// shield points
		// number of possibilities for event
		// probability distribution visualization
		for (int y=0; y<=RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides); y++) 
		{
			// dmg
			for (int x=0; x<=maxDmg; x++) 
			{
				if (RNG.getEventCount(enemy.dice, enemy.sides)[x] > y) 
				{
					GUIStyle style = normalStyle;
					if (x<=shield)
					{
						style = shieldStyle;
					}
					if (dmgX == x && dmgY == y)
					{
						style = dmgStyle;
					}
					GUI.Label (new Rect(graphWindow.x + (x-1)*(graphWindow.width/maxDmg), graphWindow.y + y*(graphWindow.height/RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides)), graphWindow.width/maxDmg, graphWindow.height/RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides)), ""+(x), style);
				}
			}
		}
	}*/

	void spawnEnemy () 
	{
		enemy = new Enemy ();
		enemy.dice = enemyDice;
		enemy.sides = enemySides;

		// randomize values for next enemy
		// take into account round?
		enemyDice = Random.Range (1, 4);
		enemySides = Random.Range (2, 6);
	}

	IEnumerator roll () 
	{
		rolling = true;
		int maxDmg = enemy.dice * enemy.sides;
		// do roll
		dmg = enemy.attack ();

		int[] eventCount = RNG.getEventCount(enemy.dice, enemy.sides);
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
		Debug.Log ("stopHeight: " + stopHeight);
		// ding, ding, ding, ding, ding
		while (true) {
			if (dmgX == dmg && dmgY == stopHeight && cyclesLeft <= 0) 
			{
				break;
			}
			if (dmgX < enemy.dice) 
			{
				dmgX = enemy.dice-1;
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
					dmgX = enemy.dice;
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
			player.water += enemy.loot;
		}
		// reset shield and pot (and rolling)
		shieldSlider.value = 0;
		shield = 0;
		pot = 0;
		rolling = false;
		// reset pointer
		dmgX = 0;
		dmgY = 0;
		updatePointerPosition ();
		pointer.enabled = false;
		// spawn new enemy
		spawnEnemy ();
		// update slider dimensions
		updateSliderDimensions ();
		// update graph
		drawGraph ();
	}
}
