using UnityEngine;
using System.Collections;

public class DungeonState : MonoBehaviour, IDuneonMenuMessageTarget
{

	public void OnLeave ()
	{
		Debug.Log ("leaving ...");
	}

	public void OnProceed ()
	{
		Debug.Log ("proceding ...");
	}
}
