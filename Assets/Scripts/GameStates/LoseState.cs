using UnityEngine;
using System.Collections;

public class LoseState : MonoBehaviour {

	// losing stuff
	// animation for suffocating player probably
	void Update ()
	{
		// test
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			Application.LoadLevel (0);
		}
	}
}
