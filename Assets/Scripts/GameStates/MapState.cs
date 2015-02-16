using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapState : MonoBehaviour 
{
	public WorldMap map;

	public ShaderFogOfWar fogOfWar;

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
		// center camera
		Camera.main.transform.position = new Vector3 (player.gameObject.transform.position.x, player.gameObject.transform.position.y, Camera.main.transform.position.z);
		updateFogOfWar ();
	}

	void Update ()
	{
		if (Input.GetButtonDown("Jump") && map.objects[(int)player.position.x,(int)player.position.y] >= 0)
		{
			gamestate.chooseEvent (map.tiles[(int)player.position.x,(int)player.position.y], map.objects[(int)player.position.x,(int)player.position.y]);
		}
		RaycastHit hitInfo;

		if (Physics.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, out hitInfo))
		{
			// 0/0 is center of map, -1/-1 is lower left corner, 1/1 is upper left corner and so on
			Vector2 hitPoint = hitInfo.point;
			/*hitPoint.x /= map.gameObject.collider.bounds.extents.x;
			hitPoint.y /= map.gameObject.collider.bounds.extents.y;*/
			Vector2 tileNr = map.worldPointToTile(new Vector2 (hitPoint.x, hitPoint.y));

			if (map.isNeighbour(player.position, tileNr)) 
			{
				fogOfWar.highlightTile ((int)tileNr.x, (int)tileNr.y);
				if (Input.GetButtonDown ("Fire1"))
				{
					player.goToTile (tileNr, map);
					updateFogOfWar ();
					gamestate.chooseEvent (map.tiles[(int)tileNr.x,(int)tileNr.y], map.objects[(int)tileNr.x,(int)tileNr.y]);
					player.consumeResources ();
				}
			}
			else 
			{
				fogOfWar.highlightTile (-1, -1);
			}
		}
	}

	void updateFogOfWar ()
	{
		Vector2[] neighbours = map.getNeighbours (player.position);
		// here it gets stupid. tiles are cleared multiple times (which is pretty fast, but should be changed when increasing field of vision).
		Vector2[] remoteNeighbours;
		foreach (Vector2 neighbour in neighbours)
		{
			fogOfWar.clearFogTile ((int)neighbour.x, (int)neighbour.y);
			remoteNeighbours = map.getNeighbours (neighbour);
			foreach (Vector2 remoteNeighbour in remoteNeighbours)
			{
				fogOfWar.clearFogTile ((int)remoteNeighbour.x, (int)remoteNeighbour.y);
				// xD
				foreach (Vector2 veryRemoteNeighbour in map.getNeighbours (remoteNeighbour))
				{
					fogOfWar.clearFogTile ((int)veryRemoteNeighbour.x, (int)veryRemoteNeighbour.y);
				}
			}
		}

	}
}
