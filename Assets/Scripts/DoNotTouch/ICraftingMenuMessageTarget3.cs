using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public interface ICraftingMenuMessageTarget : IEventSystemHandler
{
	// functions that can be called via the messaging system
	void OnCraftMedipack();
	void OnCraftInventorySlot();
	void OnCraftVisionUpgrade();
	void OnCraftpermashield();
}
