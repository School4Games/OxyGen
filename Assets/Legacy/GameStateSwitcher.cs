using UnityEngine;
using System.Collections;

public class GameStateSwitcher : MonoBehaviour {

	// remove this script after MS3

	public GameObject mapState;

	public GameObject fightState;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M))
		{
			mapState.SetActive(!mapState.activeSelf);
			fightState.SetActive(!fightState.activeSelf);
		}
	}
}
