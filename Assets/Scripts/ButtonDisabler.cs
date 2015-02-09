using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// what a horrible name
// also: what a horrible way of doing this
public class ButtonDisabler : MonoBehaviour {

	public int scrapCost;

	public Player player;

	// Update is called once per frame
	void Update () {
		if (player.inventory.resources[(int)Resource.Type.Scrap].amount < scrapCost)
		{
			GetComponent<Button>().interactable = false;
		}
	}
}
