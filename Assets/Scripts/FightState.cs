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
		shield = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect(270, 10, 120, 20), shield, 0, maxDmg));

		GUI.Label (new Rect(140, 40, 120, 20), "Bet: " + pot);
		pot = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect(270, 40, 120, 20), pot, 0, player.water - shield));

		if (GUI.Button (new Rect(400, 10, 120, 50), "Hit me!"))
		{
			StartCoroutine ("roll");
		}

		// background (graph window)
		GUI.Box (new Rect(graphWindow.x -10, graphWindow.y -10, graphWindow.width + 20, graphWindow.height +20), "");
		// shield points
		// number of possibilities for event
		for (int y=0; y<=6; y++) 
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
					GUI.Button (new Rect(graphWindow.x + (x-1)*(graphWindow.width/maxDmg), graphWindow.y + y*(graphWindow.height/6), graphWindow.width/maxDmg, graphWindow.height/6), ""+(x), style);
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

		// cycle
		bool stopped = false; 
		while (!stopped) {
			if (dmgY < 6) {
				dmgY++;
				if (dmgX = dmg) 
				{

				}
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
