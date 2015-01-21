using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// what tile is the player on?
	public Vector2 position = Vector2.zero;

	public int inventorySlots = 5;

	// make resource class?
	public Resource water;

	public Resource oxygen;

	public Resource scrap;

	public Resource parts;

	public Resource health;

	public InventoryDisplay inventoryDisplay;

	void Start ()
	{
	
	}

	// test
	// assumes that player only moves 1 tile
	// has to do check if moving to tile is possible
	// probably return true on success for feedback to wherever this is called from
	public void goToTile (Vector2 tileNr, WorldMap map)
	{
		position = tileNr;
		Vector2 worldPosition = map.tileToWorldPoint(tileNr);
		transform.position = new Vector3 (worldPosition.x, worldPosition.y, transform.position.z);
	}

	public void consumeResources ()
	{
		water.amount--;
		oxygen.amount--;
	}

	public void addResource (Resource resource, int amount)
	{
		// add
		resource.amount += amount;
		
		int occupiedSlots = 0;
		occupiedSlots += Mathf.CeilToInt ((float)water.amount / (float)water.stackSize);
		occupiedSlots += Mathf.CeilToInt ((float)oxygen.amount / (float)oxygen.stackSize);
		occupiedSlots += Mathf.CeilToInt ((float)scrap.amount / (float)scrap.stackSize);
		occupiedSlots += Mathf.CeilToInt ((float)parts.amount / (float)parts.stackSize);
		occupiedSlots += Mathf.CeilToInt ((float)health.amount / (float)health.stackSize);

		// throw out resources that don't fit in inventory
		while (occupiedSlots > inventorySlots)
		{
			// lazy
			resource.amount--;
		}

		// update display
	}
}

