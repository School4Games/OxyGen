using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public delegate void AITurn(Vector3 target);
	public static event AITurn OnAITurn;

	public Transform player;

	bool pressedButton = false;
	
	void Update () {
		if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !pressedButton) {
			OnAITurn(player.position);
			pressedButton = true;
		}
		else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
			pressedButton = false;
		}
	}
}
