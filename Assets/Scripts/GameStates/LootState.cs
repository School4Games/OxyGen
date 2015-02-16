using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LootState : MonoBehaviour, ILootMenuMessageTarget
{

	public GameState gamestate;

	public Inventory lootInventory;
	
	public void OnLeave ()
	{
		// clear inventory
		foreach (Resource resource in lootInventory.resources)
		{
			resource.amount = 0;
		}

		gamestate.looting = false;
		gamestate.switchState ();
	}
}
