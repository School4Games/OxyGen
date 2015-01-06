using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{

	public GameObject mapState;
	public WorldMap worldMap;
	
	public GameObject fightState;

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


			// resource events
			Debug.Log ("choosing event for " + terrain + ", " + obj);
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

	public void switchState ()
	{
		mapState.SetActive(!mapState.activeSelf);
		fightState.SetActive(!fightState.activeSelf);
	}
}
