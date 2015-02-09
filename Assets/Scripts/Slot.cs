﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

	public Text text;

	public Image fill;

	// storing data in two places is bad, mkay ...
	public Resource content;

	// the inventory this slot belongs to
	Inventory inventory; 

	Vector3 oldposition;

	void Awake ()
	{

	}

	public void update (Resource resource)
	{
		content = resource;
		text.text = resource.type + "\n" + resource.amount;
		text.color = resource.color;
		text.rectTransform.anchorMax = Vector2.one;
		fill.rectTransform.anchorMax = new Vector2 (1, (float)resource.amount/(float)resource.stackSize);
		fill.rectTransform.offsetMax = Vector2.zero;
		fill.rectTransform.offsetMin = Vector2.zero;
		// The background color is the same as the main color, but lighter - GDD -
		fill.color = new Color (resource.color.r+0.5f, resource.color.g+0.5f, resource.color.b+0.5f);
	}


	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		if (inventory == null)
		{
			inventory = transform.parent.GetComponent<Inventory>();
		}
		oldposition = transform.position;
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		// buggy. ask exactly what desired behaviour is before fixing
		/*text.transform.position = eventData.position;
		fill.transform.position = eventData.position;*/
	}

	#endregion

	#region IEndDragHandler implementation
	
	public void OnEndDrag (PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		/*text.transform.position = oldposition;
		fill.transform.position = oldposition;
		fill.rectTransform.offsetMax = Vector2.zero;
		fill.rectTransform.offsetMin = Vector2.zero;*/
	}
	
	#endregion

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		Slot draggedSlot = eventData.pointerDrag.GetComponent<Slot>();
		if (inventory == null)
		{
			inventory = transform.parent.GetComponent<Inventory>();
		}
		inventory.addResource(draggedSlot.content.type, draggedSlot.content.amount);
		draggedSlot.inventory.addResource(draggedSlot.content.type, -draggedSlot.content.amount);
	}

	#endregion
}
