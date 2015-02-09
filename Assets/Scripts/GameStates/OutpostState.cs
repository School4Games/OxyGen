using UnityEngine;
using System.Collections;

public class OutpostState : MonoBehaviour, IOutpostMenuMessageTarget {

	public GameState gamestate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IOutpostMenuMessageTarget implementation

	public void OnLeave ()
	{
		gamestate.outposting = false;
		gamestate.switchState ();
	}

	#endregion
}
