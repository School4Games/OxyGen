using UnityEngine;
using System.Collections;
using System;

public class Inventory : MonoBehaviour 
{
	// always in sync with existing resources
	public Resource[] resources = new Resource[5];

	public GameObject slotPrefab;

	Slot[] slots = new Slot[5];

	// Use this for initialization
	void Start () 
	{
		updateSlotNumber (slots.Length);
		updateSlotSizes ();
		updateSlotVisuals ();
	}

	public void addResource (Resource.Type type, int amount)
	{
		// add
		resources[(int)type].amount += amount;
		
		int occupiedSlots = 0;
		foreach (Resource resource in resources)
		{
			occupiedSlots += Mathf.CeilToInt ((float)resource.amount / (float)resource.stackSize);
		}
		
		// throw out resources that don't fit in inventory
		while (occupiedSlots > slots.Length)
		{
			// lazy
			resources[(int)type].amount--;
			foreach (Resource resource in resources)
			{
				occupiedSlots += Mathf.CeilToInt ((float)resource.amount / (float)resource.stackSize);
			}
		}
		
		// update display
		updateSlotVisuals ();
	}

	public void updateSlotNumber (int newNumber) {
		Array.Resize(ref slots, newNumber);
		for (int i=0; i<slots.Length; i++)
		{
			if (slots[i] == null)
			{
				GameObject newSlot = (GameObject) Instantiate (slotPrefab);
				RectTransform newSlotRect = newSlot.GetComponent<RectTransform>();
				newSlotRect.transform.SetParent(transform, false);
				slots[i] = newSlot.GetComponent<Slot>();
			}
		}
		updateSlotSizes();
	}

	public void updateSlotVisuals ()
	{
		int currentResourceIndex = 0;
		// amount left to put into slots
		int amount = resources[currentResourceIndex].amount;
		foreach (Slot slot in slots)
		{
			while (amount <= 0)
			{
				currentResourceIndex++;
				if (currentResourceIndex >= resources.Length)
				{
					break;
				}
				amount = resources[currentResourceIndex].amount;
			}
			if (currentResourceIndex < resources.Length)
			{
				int stackSize = resources[currentResourceIndex].stackSize;
				Color color = resources[currentResourceIndex].color;
				slot.update(resources[currentResourceIndex].ToString() + "\n" + Mathf.Min(amount, stackSize), (float)Mathf.Min(amount, stackSize)/(float)stackSize, color);
				amount -= stackSize;
			}
			// empty slot
			else
			{
				slot.GetComponent<Slot>().update("-", 1, Color.gray);
			}
		}
	}

	void updateSlotSizes ()
	{
		for (int i=0; i<slots.Length; i++)
		{
			// set anchors to right percentage
			RectTransform newSlotRect = slots[i].gameObject.GetComponent<RectTransform>();
			newSlotRect.anchorMin = new Vector2 ((float)i/(float)slots.Length, 0);
			newSlotRect.anchorMax = new Vector2 (((float)i+1)/(float)slots.Length, 1);
			// make graphics fit anchors
			newSlotRect.sizeDelta = Vector2.one;
		}
	}
}
