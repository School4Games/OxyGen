using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour {

	Texture2D main;

	public int mapWidth;
	public int mapHeight;

	public int[,] tiles;
	public int[,] objects;
	public SpriteRenderer[,] objectGraphics; 

	public Terrain[] terrains;

	public GameObject[] objectPrefabs = new GameObject[4];

	public ShaderFogOfWar fogOfWar;

	public GameState gameState;
		
	public Texture2D stamp;
	public int hexSideLength = 64;

	Color[] black;

	enum OffsetMode {fromZero, fromCenter};

	public bool generated = false;

	// not the best name
	GameObject objectContainer;

	// test
	public Text progressText;
	public Slider progressBar;

	void Start ()
	{
		main = (Texture2D)renderer.sharedMaterial.mainTexture;
		tiles = new int[mapWidth,mapHeight];
		objects = new int[mapWidth,mapHeight];
		objectGraphics = new SpriteRenderer[mapWidth,mapHeight];
		if (!generated)
		{
			generate ();
		}
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

	void clearObjects ()
	{
		// mwahaha
		DestroyImmediate (objectContainer);
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
			Color[] brushColors = terrains[texture].texture.GetPixels ((int)startCorner.x, (int)startCorner.y, stamp.width, stamp.height);
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

	public void generate () 
	{
		gameState.playLoop (gameState.waitLoop);
		if (!main)
		{
			main = (Texture2D)renderer.sharedMaterial.mainTexture;
		}
		// test (savegames)
		//Random.seed = 216389;
		clearTexture ();
		progressText.text = "Placing tiles ...";
		progressBar.value = 0;
		clearObjects ();
		StartCoroutine ("placeTiles");
	}

	IEnumerator createFogOfWar ()
	{
		Vector2[,] tilepositions = new Vector2[tiles.GetUpperBound(0),tiles.GetUpperBound(1)];
		for (int y=0; y<tiles.GetUpperBound(1); y++)
		{
			progressBar.value+=1.0f/(float)tiles.GetUpperBound(1);
			yield return new WaitForEndOfFrame ();
			for (int x=0; x<tiles.GetUpperBound(0); x++)
			{
				tilepositions[x,y] = tileToWorldPoint (new Vector2 (x, y));
			}
		}
		fogOfWar.createFogMesh (tilepositions);

		gameState.OnFinishedGenerating ();
		generated = true;
		gameState.switchState ();
		gameState.playLoop (gameState.mapLoop);
	}

	IEnumerator placeTiles ()
	{
		// fill array
		// test
		for (int y=0; y<tiles.GetUpperBound(1); y++) 
		{
			progressBar.value+=0.5f/(float)tiles.GetUpperBound(1);
			yield return new WaitForEndOfFrame ();
			for (int x=0; x<tiles.GetUpperBound(0); x++) 
			{
				// test
				float distanceFromCenter = (float)Mathf.Abs (x-((tiles.GetUpperBound(0)+1)/2))/(float)tiles.GetUpperBound(0);
				distanceFromCenter += (float)Mathf.Abs (y-((tiles.GetUpperBound(1)+1)/2))/(float)tiles.GetUpperBound(1);
				distanceFromCenter = Mathf.Pow (distanceFromCenter, 0.5f);

				int type = 0;
				float randomness = (Random.value-0.5f)/4;
				if (distanceFromCenter+randomness > 0.5f)
				{
					type = 1;
				}
				if (distanceFromCenter+randomness > 0.7f)
				{
					type = 2;
				}
				tiles[x,y] = type;
			}
		}
		
		// paint
		for (int y=0; y<tiles.GetUpperBound(1); y++) 
		{
			progressBar.value+=0.5f/(float)tiles.GetUpperBound(1);
			yield return new WaitForEndOfFrame ();
			for (int x=0; x<tiles.GetUpperBound(0); x++) 
			{
				Vector2 center = getHexCenter (x, y);
				// test
				center += getOffset (OffsetMode.fromZero);
				placeTile(center, tiles[x,y]);
			}
		}

		// apply
		main.Apply();

		progressText.text = "Placing objects ...";
		progressBar.value = 0;
		StartCoroutine ("placeObjects");
	}

	IEnumerator placeObjects ()
	{
		// fill array
		// test
		for (int y=0; y<objects.GetUpperBound(1); y++) 
		{
			progressBar.value+=0.5f/(float)tiles.GetUpperBound(1);
			yield return new WaitForEndOfFrame ();
			for (int x=0; x<objects.GetUpperBound(0); x++) 
			{
				// test
				objects[x,y] = Random.Range(1, 3);
				if (Random.value < 0.8f)
				{
					objects[x,y] = -1;
				}
			}
		}
		// place 4 parts
		int i = 4;
		while (i>0)
		{
			int x = Random.Range(0, mapWidth);
			int y = Random.Range(0, mapHeight);
			if (objects[x,y]<0)
			{
				objects[x,y] = 3;
				i--;
			}
		}
		// place base in center
		objects[mapWidth/2, mapHeight/2] = 0;

		// place
		objectContainer = new GameObject ();
		objectContainer.transform.parent = transform;
		for (int y=0; y<objects.GetUpperBound(1); y++) 
		{
			progressBar.value+=0.5f/(float)tiles.GetUpperBound(1);
			yield return new WaitForEndOfFrame ();
			for (int x=0; x<objects.GetUpperBound(0); x++) 
			{
				if (objects[x,y] >= 0)
				{
					GameObject placedObject = (GameObject) Instantiate (objectPrefabs[objects[x,y]], new Vector3 (0,0,-1), Quaternion.identity);
					objectGraphics[x,y] = placedObject.GetComponent<SpriteRenderer>();
					Vector2 worldPosition = tileToWorldPoint(new Vector2 (x, y));
					placedObject.transform.parent = objectContainer.transform;
					placedObject.transform.position = new Vector3 (worldPosition.x, worldPosition.y, placedObject.transform.position.z);
				}
			}
		}

		progressText.text = "Creating fog of war ...";
		progressBar.value = 0;
		StartCoroutine ("createFogOfWar");
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

	// returns all immediate neighbours including the tile itself
	public Vector2[] getNeighbours (Vector2 tileNr)
	{
		ArrayList neighbours = new ArrayList ();

		neighbours.Add (new Vector2 ((int)tileNr.x, (int)tileNr.y));
		neighbours.Add (new Vector2 ((int)tileNr.x+1, (int)tileNr.y));
		neighbours.Add (new Vector2 ((int)tileNr.x-1, (int)tileNr.y));
		neighbours.Add (new Vector2 ((int)tileNr.x, (int)tileNr.y+1));
		neighbours.Add (new Vector2 ((int)tileNr.x, (int)tileNr.y-1));
		
		// even
		if (tileNr.x % 2 == 0)
		{
			neighbours.Add (new Vector2 ((int)tileNr.x-1, (int)tileNr.y-1));
			neighbours.Add (new Vector2 ((int)tileNr.x+1, (int)tileNr.y-1));
		}
		// odd
		else 
		{
			neighbours.Add (new Vector2 ((int)tileNr.x-1, (int)tileNr.y+1));
			neighbours.Add (new Vector2 ((int)tileNr.x+1, (int)tileNr.y+1));
		}
		Vector2[] neighboursArray = new Vector2[neighbours.Count];
		neighbours.CopyTo (neighboursArray);
		return neighboursArray;
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
