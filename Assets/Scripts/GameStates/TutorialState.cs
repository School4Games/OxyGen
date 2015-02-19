using UnityEngine;
using System.Collections;

public class TutorialState : MonoBehaviour {
	
	public GameObject[] messages;

	public ArrayList test = new ArrayList ();

	public void enableMessage (int messageID)
	{
		disableAllMessages ();
		messages[messageID].SetActive (true);
	}

	public void disableAllMessages ()
	{
		foreach (GameObject message in messages)
		{
			message.SetActive (false);
		}
	}
}
