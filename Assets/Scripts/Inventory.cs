using UnityEngine;
using System.Collections;
using System;

public class Inventory : MonoBehaviour
{
	// always in sync with existing resources
	public Resource[] resources = new Resource[5];

	public GameObject slotPrefab;

	public int slotNumber = 5;

	Slot[] slots = new Slot[5];

	// Use this for initialization
	void Awake () 
	{
		updateSlotNumber (slotNumber);
		updateSlotVisuals ();
	}

	public void OnClickSlot ()
	{
		// start dragging
		if (GameState.draggedResource == null)
		{
			pickUp ();
		}
		// drop
		else 
		{
			drop ();
		}
	}

	public void pickUp ()
	{

	}

	public void drop ()
	{
		// unsafe. destroys resources, when they don't fit.
		addResource (GameState.draggedResource.type, GameState.draggedResource.amount);
		GameState.draggedResource = null;
	}

	// >_<
	public Slot[] zloty 
	{
		get 
		{
			return slots;
		}
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
		slotNumber = slots.Length;
	}

	public void updateSlotVisuals ()
	{
		int currentResourceIndex = 0;
		// amount left to put into slots
		int amount = resources[currentResourceIndex].amount;
		// hmmm ...
		if (slots[0] == null)
		{
			updateSlotNumber(slots.Length);
		}
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

				// is there a better way to do this?
				Resource resource = new Resource();
				resource.amount = amount;
				resource.color = resources[currentResourceIndex].color;
				resource.stackSize = resources[currentResourceIndex].stackSize;
				resource.type = resources[currentResourceIndex].type;
				resource.amount = Mathf.Min (resource.amount, stackSize);

				slot.update(resource);
				amount -= stackSize;
			}
			// empty slot
			else
			{
				slot.GetComponent<Slot>().update(new Resource());
			}
		}
	}
}
