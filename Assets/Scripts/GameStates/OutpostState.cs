using UnityEngine;
using System.Collections;

public class OutpostState : MonoBehaviour, IOutpostMenuMessageTarget {

	public GameState gamestate;

	void Awake ()
	{
		gamestate.tutorialState.enableMessage (4);
	}

	#region IOutpostMenuMessageTarget implementation

	public void OnLeave ()
	{
		gamestate.tutorialState.disableAllMessages ();
		gamestate.outposting = false;
		gamestate.switchState ();
	}

	public void OnOxygenRefill ()
	{
		gamestate.player.inventory.addResource(Resource.Type.Oxygen, 1);
		gamestate.playSound (gamestate.oxygenRefillSound);
	}

	#endregion
}
