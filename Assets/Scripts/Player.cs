using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// what tile is the player on?
	Vector2 position = Vector2.zero;

	public int health = 3;

	public int water = 50;

	void Start ()
	{
		goToTile (0, 0);
	}

	// test
	public void goToTile (float x, float y)
	{
		position.x = x;
		position.y = y;
		transform.position = new Vector3 (x, y, transform.position.z);
	}
}

