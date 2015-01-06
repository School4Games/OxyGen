using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapControls : MonoBehaviour 
{

	public WorldMap map;

	public GameState gamestate;

	public Player player;

	public Text water;

	public Text oxygen;

	public Text health;

	void Start ()
	{
		Vector2 basePosition = Vector2.zero;
		for (int y=0; y<map.objects.GetUpperBound(1); y++)
		{
			for (int x=0; x<map.objects.GetUpperBound(0); x++)
			{
				if (map.objects[x,y] == 0)
				{
					basePosition = new Vector2 (x,y);
				}
			}
		}
		player.goToTile (basePosition, map);
	}

	void Update ()
	{
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
				Vector2 tileNr = map.worldPointToTile(new Vector2 (hitPoint.x, hitPoint.y));

				if (map.isNeighbour(player.position, tileNr)) 
				{
					player.goToTile (tileNr, map);
					gamestate.chooseEvent (map.tiles[(int)tileNr.x,(int)tileNr.y], map.objects[(int)tileNr.x,(int)tileNr.y]);
					player.consumeResources ();
				}
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
