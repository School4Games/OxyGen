using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

	public GameObject slotPrefab;

	public Player player;

	public ArrayList slots = new ArrayList();

	public GameObject hotbar;

	void Start ()
	{
		updateSlotNumber ();
		updateSlotContent ();
	}

	public void update ()
	{
		updateSlotNumber ();
		updateSlotContent ();
	}

	void updateSlotNumber () {
		while (slots.Count < player.inventorySlots)
		{
			GameObject newSlot = (GameObject) Instantiate (slotPrefab);

			RectTransform newSlotRect = newSlot.GetComponent<RectTransform>();
			
			newSlotRect.transform.SetParent(hotbar.transform, false);

			slots.Add (newSlot);

			updateSlotSizes ();
		}
	}

	void updateSlotSizes ()
	{
		for (int i=0; i<slots.Count; i++)
		{
			// set anchors to right percentage 
			RectTransform newSlotRect = (slots[i] as GameObject).GetComponent<RectTransform>();
			newSlotRect.anchorMin = new Vector2 ((float)i/(float)player.inventorySlots, 0);
			newSlotRect.anchorMax = new Vector2 (((float)i+1)/(float)player.inventorySlots, 1);
			
			// make graphics fit anchors
			newSlotRect.sizeDelta = Vector2.one;
		}
	}

	void updateSlotContent ()
	{
		foreach (GameObject slot in slots)
		{
			slot.GetComponent<Slot>().update("-", 1, Color.gray);
		}
		int currentSlot = 0;

		// >_<
		int water = player.water.amount;
		while (water > 0)
		{
			(slots[currentSlot] as GameObject).GetComponent<Slot>().update("H2O", (float)Mathf.Min(water, player.water.stackSize)/(float)player.water.stackSize, player.water.color);
			water -= player.water.stackSize;
			currentSlot++;
		}

		int oxygen = player.oxygen.amount;
		while (oxygen > 0)
		{
			(slots[currentSlot] as GameObject).GetComponent<Slot>().update("O2", (float)Mathf.Min(oxygen, player.oxygen.stackSize)/(float)player.oxygen.stackSize, player.oxygen.color);
			oxygen -= player.oxygen.stackSize;
			currentSlot++;
		}
	}
}
