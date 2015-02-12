﻿using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

	Texture2D main;

	public int[,] tiles = new int[16,16];

	public Player player;
		
	public Texture2D stamp;
	public int hexSideLength = 64;

	Color[] black;

	enum OffsetMode {fromZero, fromCenter};

	void Start ()
	{
		main = (Texture2D)renderer.sharedMaterial.mainTexture;
		clearTexture ();

		// test 
		paint ((int)player.position.x, (int)player.position.y);
	}

	public void paint (int x, int y)
	{
		Vector2 center = getHexCenter (x, y);
		// test
		center += getOffset (OffsetMode.fromZero);
		placeTile(center);
		
		// apply
		main.Apply();
	}

	void clearTexture ()
	{
		if (black == null)
		{
			black = new Color[main.width*main.height];
			for (int i=0; i<black.Length; i++)
			{
				black[i] = Color.black;
			}
		}
		main.SetPixels (black);
		main.Apply();
	}

	// performance critical function
	// => don't do weird stuff here (or save beforehand)
	// also optimize when freetime available
	void placeTile (Vector2 center)
	{
		if (stamp != null) {
			// calculate x and y coordinate from center and stamp dimensions
			Vector2 startCorner = center - new Vector2(stamp.width/2, stamp.height/2);

			// calculate brush colors from stamp grayscale (as alpha) and layer texture
			Color[] brushColors = new Color[stamp.width * stamp.height];
			// get once and save?
			Color[] stampColors = stamp.GetPixels ();
			Color[] mapColors = main.GetPixels ((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height);
			// calculate actual color to paint on map
			for (int i=0; i<brushColors.Length; i++)
			{
				brushColors[i] = new Color(mapColors[i].r, mapColors[i].g, mapColors[i].b, mapColors[i].a - (stampColors[i].grayscale));
			}

			// draw on texture
			main.SetPixels((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height, brushColors);
		}
	}

	Vector2 getHexCenter (int x, int y)
	{
		Vector2 center = Vector2.zero;

		// height of a regular triangle with hexSideLength as side length
		float h = Mathf.Pow (3.0f, 0.5f) / 2 * hexSideLength;

		center.x += (1.5f * hexSideLength) * x;

		// offset every second column
		if ((x+1) % 2 == 0)
		{
			center.y += h;
		}

		center.y += (2 * h) * y;

		return center;
	}

	// probably save result?
	// I somehow got the center wrong here ...
	Vector2 getOffset (OffsetMode offsetMode)
	{
		if (!main)
		{
			main = (Texture2D)renderer.sharedMaterial.mainTexture;
		}
		Vector2 offset = Vector2.zero;

		// height of a regular triangle with hexSideLength as side length
		float h = Mathf.Pow (3.0f, 0.5f) / 2 * hexSideLength;

		offset.x += tiles.GetUpperBound (0) / 2 * hexSideLength;

		offset.y += tiles.GetUpperBound (1) * h;

		Vector2 center = new Vector2 (main.width/2, main.height/2);

		if (offsetMode == OffsetMode.fromCenter) 
		{
			offset = - offset;
		}
		else
		{
			offset = center - offset;
		}

		return offset;
	}

	void placeTiles ()
	{
		// fill array
		// test
		for (int y=0; y<tiles.GetUpperBound(1); y++) 
		{
			for (int x=0; x<tiles.GetUpperBound(0); x++) 
			{
				// test
				tiles[x,y] = Random.Range(0, 3);
			}
		}
		
		// paint
		for (int y=0; y<tiles.GetUpperBound(1); y++) 
		{
			for (int x=0; x<tiles.GetUpperBound(0); x++) 
			{
				Vector2 center = getHexCenter (x, y);
				// test
				center += getOffset (OffsetMode.fromZero);
				placeTile(center);
			}
		}

		// apply
		main.Apply();
	}

	public bool isNeighbour (Vector2 tile1Nr, Vector2 tile2Nr)
	{
		bool neighbour = false;
		// cardinal directions
		if ((Mathf.Abs(tile2Nr.x - tile1Nr.x) + Mathf.Abs(tile2Nr.y - tile1Nr.y)) == 1)
		{
			neighbour = true;
		}

		// diagonals

		// even
		else if ((tile2Nr.x + 1) % 2 == 0)
		{
			if (Mathf.Abs(tile2Nr.x - tile1Nr.x) == 1 && (tile2Nr.y - tile1Nr.y) == -1)
			{
				neighbour = true;
			}
		}
		// odd
		else if ((tile2Nr.x + 1) % 2 != 0)
		{
			if (Mathf.Abs(tile2Nr.x - tile1Nr.x) == 1 && (tile2Nr.y - tile1Nr.y) == 1)
			{
				neighbour = true;
			}
		}

		return neighbour;
	}

	public Vector2 worldPointToTile (Vector2 worldPoint)
	{
		Vector2 tileNr = Vector2.zero;

		// relative to center
		worldPoint.x += collider.bounds.extents.x;
		worldPoint.y += collider.bounds.extents.y;

		// w.o. scale
		worldPoint.x /= collider.bounds.size.x;
		worldPoint.y /= collider.bounds.size.y;

		// in pixels
		worldPoint.x *= main.width;
		worldPoint.y *= main.height;

		worldPoint -= getOffset (OffsetMode.fromZero);

		// reverse getHexCenter
		tileNr.x = worldPoint.x / (1.5f * hexSideLength);
		tileNr.x = Mathf.Round (tileNr.x);

		// height of a regular triangle with hexSideLength as side length
		float h = Mathf.Pow (3.0f, 0.5f) / 2 * hexSideLength;

		if ((tileNr.x+1) % 2 == 0) 
		{
			worldPoint.y -= h;
		}

		tileNr.y = worldPoint.y / (2 * h);
		tileNr.y = Mathf.Round (tileNr.y);

		return tileNr;
	}

	public Vector2 tileToWorldPoint (Vector2 tileNr)
	{
		// in pixels
		Vector2 worldPoint = getHexCenter ((int)tileNr.x, (int)tileNr.y);
		worldPoint += getOffset (OffsetMode.fromZero);

		// in units
		worldPoint.x /= main.width;
		worldPoint.y /= main.height;

		// w. scale
		worldPoint.x *= collider.bounds.size.x;
		worldPoint.y *= collider.bounds.size.y;

		// relative to lower left map corner
		worldPoint.x -= collider.bounds.extents.x;
		worldPoint.y -= collider.bounds.extents.y;

		return worldPoint;
	}
}