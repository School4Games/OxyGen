using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapControls : MonoBehaviour 
{

	public WorldMap map;

	public Player player;

	public Text water;

	public Text oxygen;

	public Text health;

	void Start ()
	{
		player.goToTile (new Vector2 (0, 1), map);
	}

	void Update ()
	{
		// test
		updateStatsDisplay ();

		RaycastHit hitInfo;
		if (Input.GetButtonDown ("Fire1"))
		{
			if (Physics.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, out hitInfo))
			{
				// 0/0 is center of map, -1/-1 is lower left corner, 1/1 is upper left corner and so on
				Vector2 hitPoint = hitInfo.point;
				/*hitPoint.x /= map.gameObject.collider.bounds.extents.x;
				hitPoint.y /= map.gameObject.collider.bounds.extents.y;*/

				player.goToTile (map.worldPointToTile(new Vector2 (hitPoint.x, hitPoint.y)), map);
				player.consumeResources ();
			}
		}
	}

	void updateStatsDisplay ()
	{
		water.text = " h2o: " + player.water; 
		oxygen.text = " o2: " + player.oxygen;
		health.text = " hp: " + player.health;
	}
}
