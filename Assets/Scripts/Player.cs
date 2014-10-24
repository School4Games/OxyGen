using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	bool pressedButton = false;

	void Update () {
		if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !pressedButton) {
			transform.position += (Vector3.right*Input.GetAxis("Horizontal")).normalized;
			transform.position += (Vector3.up*Input.GetAxis("Vertical")).normalized;
			pressedButton = true;
		}
		else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
			pressedButton = false;
		}
	}
}

