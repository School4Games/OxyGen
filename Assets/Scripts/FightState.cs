using UnityEngine;
using System.Collections;

public class FightState : MonoBehaviour 
{

	Player player;

	Enemy enemy;

	int shield = 0;

	int dmg = -1;

	// "lootboost"
	// how do we call this?
	int pot = 0;

	// dmg pointer
	int dmgX = 0;
	int dmgY = 0;

	int roundsSurvived = 0;

	public Rect graphWindow = new Rect (10, 60, 800, 400);

	public GUIStyle shieldStyle = new GUIStyle();
	public GUIStyle normalStyle = new GUIStyle();
	public GUIStyle dmgStyle = new GUIStyle();

	// Use this for initialization
	void Start () 
	{
		// test
		// get actual player here instead of creating new one
		player = new Player ();
		enemy = new Enemy ();
		enemy.dice = 3;
		enemy.sides = 5;
		string log = "";
		// test
		/*foreach (int eventCount in RNG.getEventCount(2, 6))
		{
			log += eventCount + ", ";
		}*/
		Debug.Log(RNG.getMaximumAbsoluteProbability(enemy.dice, enemy.sides));
		Debug.Log(log);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI () 
	{
		int maxDmg = enemy.dice * enemy.sides;
		//player stats
		GUI.Label (new Rect(10, 10, 120, 20), "Health: " + player.health);
		GUI.Label (new Rect(10, 40, 120, 20), "Water: " + (player.water - shield - pot));

		GUI.Label (new Rect(140, 10, 120, 20), "Shields: " + shield);
		shield = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect(270, 10, 120, 20), shield, 0, maxDmg - pot));

		GUI.Label (new Rect(140, 40, 120, 20), "Bet: " + pot);
		pot = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect(270, 40, 120, 20), pot, 0, Mathf.Min(player.water - shield, maxDmg - shield)));

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

		GUI.Label (new Rect(530, 10, 150, 50), "Win: gain " + (pot + enemy.loot - shield) + " water");
		GUI.Label (new Rect(530, 40, 150, 50), "Lose: gain " + (-pot - shield) + " water");

		GUI.Label (new Rect(Screen.width - 160, 10, 150, 50), "You survived for " + roundsSurvived + " rounds");

		// background (graph window)
		GUI.Box (new Rect(graphWindow.x -10, graphWindow.y -10, graphWindow.width + 20, graphWindow.height +20), "");
		// shield points
		// number of possibilities for event
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
	}

	IEnumerator roll () 
	{
		int maxDmg = enemy.dice * enemy.sides;
		// do roll
		dmg = enemy.attack ();
		// confiscate stakes
		player.water -= shield;
		player.water -= pot;

		int[] eventCount = RNG.getEventCount(enemy.dice, enemy.sides);
		// cycle
		int stopHeight = Random.Range(1, eventCount[dmg]+1);
		while (true) {
			if (dmgX == dmg && dmgY == stopHeight) 
			{
				break;
			}
			if (dmgY < eventCount[dmgX]) {
				dmgY++;
				yield return new WaitForSeconds (0.009f);
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
					dmgX = 0;
				}
				yield return new WaitForSeconds (0.009f);
			}
		}
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
		// reset shield and pot
		shield = 0;
		pot = 0;
	}
}
