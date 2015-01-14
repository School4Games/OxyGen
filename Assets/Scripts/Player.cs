using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// what tile is the player on?
	public Vector2 position = Vector2.zero;

	public int maxHealth = 3;
	public int health = 3;

	public int inventorySpace = 60;
	public int water = 50;
	public int oxygen = 10;
	public int scrap = 0;
	public int parts = 0;

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
		water--;
		oxygen--;
	}
}

