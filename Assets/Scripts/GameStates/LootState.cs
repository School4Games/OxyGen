using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LootState : MonoBehaviour, ILootMenuMessageTarget
{

	public GameState gamestate;

	public Inventory lootInventory;

	void Awake ()
	{
	
	}

	public void setLootText (int amount, Resource.Type type)
	{
		lootInventory.addResource(type, amount);
	}
	
	public void OnLeave ()
	{
		gamestate.looting = false;
		gamestate.switchState ();
	}
}
