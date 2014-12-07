﻿using UnityEngine;
using System.Collections;

public class WorldMap : MonoBehaviour {

	Texture2D main;

	public int[,] tiles = new int[16,16];

	public Texture2D[] layerTextures;
		
	public Texture2D stamp;
	public int hexSideLength = 64;

	enum OffsetMode {fromZero, fromCenter};

	public bool generated = false;

	void Start ()
	{
		main = (Texture2D)renderer.sharedMaterial.mainTexture;
		if (!generated)
		{
			generate ();
		}
	}

	// performance critical function (on load)
	// => don't do weird stuff here (or save beforehand)
	// also optimize when freetime available
	void placeTile (Vector2 center, int texture)
	{
		if (stamp != null) {
			// calculate x and y coordinate from center and stamp dimensions
			Vector2 startCorner = center - new Vector2(stamp.width/2, stamp.height/2);

			// calculate brush colors from stamp grayscale (as alpha) and layer texture
			Color[] brushColors = layerTextures[texture].GetPixels ((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height);
			// get once and save?
			Color[] stampColors = stamp.GetPixels ();
			Color[] mapColors = main.GetPixels ((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height);
			// calculate actual color to paint on map
			for (int i=0; i<brushColors.Length; i++)
			{
				brushColors[i] = ((brushColors[i] * stampColors[i].grayscale) + (mapColors[i] * (1-stampColors[i].grayscale)));
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

	public void generate () 
	{
		if (!main)
		{
			main = (Texture2D)renderer.sharedMaterial.mainTexture;
		}
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
				placeTile(center, tiles[x,y]);
			}
		}

		generated = true;
		// apply
		main.Apply();
	}

	public Vector2 worldPointToTile (Vector2 worldPoint)
	{
		Vector2 tileNr = Vector2.zero;
		// calculate (reverse gethexCenter)
		tileNr.x = (worldPoint.x + collider.bounds.extents.x - getOffset(OffsetMode.fromZero).x)/(1.5f*hexSideLength);

		Debug.Log ("Tile#" + tileNr);
		return tileNr;
	}

	public Vector2 tileToWorldPoint (Vector2 tileNr)
	{
		Vector2 worldPoint = Vector2.zero;
		// fromzero because fromCenter is broken (or is it ...)
		// texture space ;)
		worldPoint = getHexCenter ((int)tileNr.x, (int)tileNr.y) + getOffset(OffsetMode.fromZero);
		// from texture to world space
		worldPoint.x /= main.width;
		worldPoint.y /= main.height;
		// sideLength in unitymeters
		//worldPoint *= (1.5f * ((float)hexSideLength/(float)main.width));
		worldPoint.x *= collider.bounds.extents.x;
		worldPoint.y *= collider.bounds.extents.y;
		worldPoint *= 1.5f;
		worldPoint -= new Vector2(collider.bounds.extents.x, collider.bounds.extents.y);

		Debug.Log ("WorldPoint: " + worldPoint + " = " 
		    + (getHexCenter ((int)tileNr.x, (int)tileNr.y) + getOffset(OffsetMode.fromZero))
			+ " / (" + main.width + ", " + main.height + ")"
			+ " * " + (1.5f * ((float)hexSideLength/(float)main.width)));
		return worldPoint;
	}
}
