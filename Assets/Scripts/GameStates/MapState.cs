using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapState : MonoBehaviour 
{
	public WorldMap map;

	public FogOfWar fogOfWar;

	public GameState gamestate;

	public Player player;

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
					fogOfWar.paint((int)player.position.x, (int)player.position.y);
					player.consumeResources ();
					gamestate.chooseEvent (map.tiles[(int)tileNr.x,(int)tileNr.y], map.objects[(int)tileNr.x,(int)tileNr.y]);
				}
			}
		}
	}
}
