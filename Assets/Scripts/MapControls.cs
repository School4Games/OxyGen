using UnityEngine;
using System.Collections;

public class MapControls : MonoBehaviour 
{

	public GameObject map;

	public Player player;

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

				player.goToTile (hitPoint.x, hitPoint.y);

				Debug.Log (hitPoint);
			}
			// Debug.DrawLine (Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward);
		}
	}
}
