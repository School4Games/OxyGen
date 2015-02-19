using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialState : MonoBehaviour 
{
	public GameObject[] messages;

	public ArrayList test = new ArrayList ();

	public Text enemyText;

	public void setEnemyText (float maxDamage)
	{
		string text = enemyText.text;
		text = text.Replace ("$maxvalue", "" + maxDamage);
		enemyText.text = text;
	}

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
