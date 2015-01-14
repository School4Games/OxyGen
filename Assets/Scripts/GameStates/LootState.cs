using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LootState : MonoBehaviour, ILootMenuMessageTarget {

	public GameState gamestate;

	public Text lootText;

	public void setLootText (int amount, ResourceEvent.Type type)
	{
		lootText.text = "You got " + amount + " " + type.ToString ();
	}
	
	public void OnLeave ()
	{
		gamestate.looting = false;
		gamestate.switchState ();
	}
}
