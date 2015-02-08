using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// what tile is the player on?
	public Vector2 position = Vector2.zero;

	public int inventorySlots = 5;

	public Inventory inventory;

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
		inventory.resources[(int)Resource.Type.Water].amount--;
		inventory.resources[(int)Resource.Type.Oxygen].amount--;
	}

	public void addResource (Resource.Type type, int amount)
	{
		inventory.addResource(type, amount);
	}
}

