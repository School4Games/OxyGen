using UnityEngine;
using System.Collections;

[System.Serializable]
public class tile {
	//note to self: spawning each tile as a gameobject (sprite) might kill performance for big maps
	//consider using plane and uvs 
	public int floor;
	public int structure;
	public int unit;
}

public class WorldMap : MonoBehaviour {

	Texture2D main;

	private tile[,] tiles;

	public int width;
	public int height;

	public Texture2D[] layerTextures;
		
	public Texture2D stamp;
	public int hexSideLength = 64;
	
	//Color[] stamp = new Color[100];

	void Start ()
	{
		// test
		placeTile (new Vector2 (200, 200), 0);
	}

	void placeTile (Vector2 center, int texture) 
	{
		if (stamp != null) {
			main = (Texture2D)renderer.material.mainTexture;

			// calculate x and y coordinate from center and stamp dimensions
			Vector2 startCorner = center - new Vector2(stamp.width/2, stamp.height/2);

			// calculate brush colors from stamp grayscale (as alpha) and layer texture
			Color[] brushColors = layerTextures[texture].GetPixels ((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height);
			// get once and save?
			Color[] stampColors = stamp.GetPixels ();
			Color[] mapColors = main.GetPixels ((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height);
			for (int i=0; i<brushColors.Length; i++)
			{
				brushColors[i] = ((brushColors[i] * stampColors[i].grayscale) + (mapColors[i] * (1-stampColors[i].grayscale)));
			}

			// calculate actual color to paint on map

			// draw on texture
			main.SetPixels((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height, brushColors);
			main.Apply();
		}
	}

	public void generate () {
		tiles = new tile[width,height];
		for (int y=0; y<height; y++) {
			for (int x=0; x<width; x++) {
				tiles[x,y] = new tile();
			}
		}
		//do generation stuff
		for (int y=0; y<height; y++) {
			for (int x=0; x<height; x++) {

			}
		}
	}
}
