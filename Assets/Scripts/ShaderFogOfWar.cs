using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShaderFogOfWar : MonoBehaviour 
{
	public Color clearColor;

	public Color highlightColor;

	Mesh mesh;

	int mapWidth;
	int mapHeight;

	int highlightedTile = -1;

	// test
	int tri;
	bool grabbed = false;

	void Start () 
	{
		mesh = GetComponent<MeshFilter> ().mesh;
	}

	void Update () 
	{
		testStuff ();
	}

	public void createFogMesh (Vector2[,] positions)
	{
		mapWidth = positions.GetUpperBound (0)+1;
		mapHeight = positions.GetUpperBound (1)+1;

		Vector3[] vertices = new Vector3[mapWidth*mapHeight+4];
		int [] triangles = new int[vertices.Length*18+24];
		Vector2[] uv = new Vector2[vertices.Length];
		for (int y=0; y<mapHeight; y++)
		{
			for (int x=0; x<mapWidth; x++)
			{
				Vector3 position = new Vector3 (positions[x,y].x, positions[x,y].y, 0);
				int index = x + y*mapWidth;
				vertices[index] = position;
				uv[index] = new Vector2 ((float)x/(float)mapWidth, (float)y/(float)mapHeight);

				if (x>0 && y<mapHeight-1 && y>0 && x<mapWidth-1)
				{
					// safety
					if (index+1+mapWidth < vertices.Length)
					{
						// complete hexagon
						// creates unnecessary tris

						// tri 1
						triangles[index*18] = index;
						// up
						triangles[index*18+1] = index+mapWidth;
						// even, upper left
						if (x % 2 == 0)
						{
							triangles[index*18+2] = index-1;
						}
						// odd, upper left
						else
						{
							triangles[index*18+2] = index-1+mapWidth;
						}

						// tri 2
						triangles[index*18+3] = index;
						// even, upper left
						if (x % 2 == 0)
						{
							triangles[index*18+4] = index-1;
						}
						// odd, upper left
						else
						{
							triangles[index*18+4] = index-1+mapWidth;
						}
						// even, lower left
						if (x % 2 == 0)
						{
							triangles[index*18+5] = index-1-mapWidth;
						}
						// odd, lower left
						else
						{
							triangles[index*18+5] = index-1;
						}

						// tri 3
						triangles[index*18+6] = index;
						// even, lower left
						if (x % 2 == 0)
						{
							triangles[index*18+7] = index-1-mapWidth;
						}
						// odd, lower left
						else
						{
							triangles[index*18+7] = index-1;
						}
						// down
						triangles[index*18+8] = index-mapWidth;

						// tri 4
						triangles[index*18+9] = index;
						// down
						triangles[index*18+10] = index-mapWidth;
						// even, lower right
						if (x % 2 == 0)
						{
							triangles[index*18+11] = index+1-mapWidth;
						}
						// odd, lower right
						else
						{
							triangles[index*18+11] = index+1;
						}

						// tri 5
						triangles[index*18+12] = index;
						// even, lower right
						if (x % 2 == 0)
						{
							triangles[index*18+13] = index+1-mapWidth;
						}
						// odd, lower right
						else
						{
							triangles[index*18+13] = index+1;
						}
						// even, upper right
						if (x % 2 == 0)
						{
							triangles[index*18+14] = index+1;
						}
						// odd, upper right
						else
						{
							triangles[index*18+14] = index+1+mapWidth;
						}

						// tri 6
						triangles[index*18+15] = index;
						// even, upper right
						if (x % 2 == 0)
						{
							triangles[index*18+16] = index+1;
						}
						// odd, upper right
						else
						{
							triangles[index*18+16] = index+1+mapWidth;
						}
						// up
						triangles[index*18+17] = index+mapWidth;
					}
				}
			}
		}

		// create border
		vertices [vertices.Length - 4] = new Vector3 (-5, -5, 0);
		vertices [vertices.Length - 3] = new Vector3 (5, -5, 0);
		vertices [vertices.Length - 2] = new Vector3 (5, 5, 0);
		vertices [vertices.Length - 1] = new Vector3 (-5, 5, 0);

		// bottom
		triangles [triangles.Length-24] = vertices.Length - 4;
		triangles [triangles.Length-23] = vertices.Length - 3;
		triangles [triangles.Length-22] = 1;
		
		triangles [triangles.Length-21] = vertices.Length - 3;
		triangles [triangles.Length-20] = 2*mapWidth-1;
		triangles [triangles.Length-19] = 1;

		// right
		triangles [triangles.Length-18] = vertices.Length - 3;
		triangles [triangles.Length-17] = vertices.Length - 2;
		triangles [triangles.Length-16] = 2*mapWidth-1;
		
		triangles [triangles.Length-15] = vertices.Length - 2;
		triangles [triangles.Length-14] = vertices.Length - 5;
		triangles [triangles.Length-13] = 2*mapWidth-1;

		// top
		triangles[triangles.Length-12] = vertices.Length - 2;
		triangles[triangles.Length-11] = vertices.Length - 4 - mapWidth;
		triangles[triangles.Length-10] = vertices.Length - 5;

		triangles[triangles.Length-9] = vertices.Length - 2;
		triangles[triangles.Length-8] = vertices.Length - 1;
		triangles[triangles.Length-7] = vertices.Length - 4 - mapWidth;

		// left
		triangles[triangles.Length-6] = vertices.Length - 1;
		triangles[triangles.Length-5] = 1;
		triangles[triangles.Length-4] = vertices.Length - 4 - mapWidth;

		triangles[triangles.Length-3] = vertices.Length - 1;
		triangles[triangles.Length-2] = vertices.Length - 4;
		triangles[triangles.Length-1] = 1;

		// test
		Color[] colors = new Color[vertices.Length];
		for (int i=0; i<colors.Length; i++)
		{
			colors[i] = Color.white;
			colors[i].a = 1;
			/*colors[i].r = Random.Range(0.5f, 1.0f);
			colors[i].g = colors[i].r;
			colors[i].b = colors[i].r;*/
		}
		/*colors [vertices.Length - 4 - mapWidth] = Color.red;
		colors [vertices.Length - 1] = Color.green;
		colors [vertices.Length - 2] = Color.blue;*/

		// test
		tri = triangles.Length-10;

		mesh.vertices = vertices;
		//mesh.uv = uv;
		mesh.triangles = triangles;

		mesh.colors = colors;
		//colorTest ();
	}

	public void clearFogTile (int x, int y)
	{
		Color[] colors = mesh.colors;

		colors[x+y*mapWidth] = clearColor;

		mesh.colors = colors;
	}

	public void highlightTile (int x, int y)
	{
		Color[] colors = mesh.colors;

		if (highlightedTile >= 0)
		{
			colors[highlightedTile] = clearColor;
		}
		if (x>=0 && y>=0)
		{
			highlightedTile = x + y * mapWidth;

			colors [highlightedTile] = highlightColor;
		}

		mesh.colors = colors;
	}

	void colorTest ()
	{
		Color[] colors = new Color[mesh.vertices.Length];
		for (int i=0; i<colors.Length; i++)
		{
			colors[i] = Color.magenta;
			colors[i].a = 1;
			colors[i].r = Random.Range(0.5f, 1.0f);
			colors[i].g = colors[i].r;
			colors[i].b = colors[i].r;
		}
		mesh.colors = colors;
	}

	void testStuff ()
	{
		// test
		Vector2 offset = Vector2.one;
		offset.x = Mathf.Sin(Time.time/30)/10;
		offset.y = Mathf.Sin(Time.time/30+1)/10;
		offset.Normalize ();
		offset *= Time.time/30;
		renderer.material.SetTextureOffset ("_MainTex", offset);
		
		// test 
		int[] triangles = mesh.triangles;
		Color[] colors = mesh.colors;
		Vector3[] vertices = mesh.vertices;
		if (Input.GetKeyDown(KeyCode.Q))
		{
			colors[triangles[tri]] = Color.white;
			tri--;
			Debug.Log (triangles[tri] + "/" + (vertices.Length-1));
			if (Input.GetKey(KeyCode.LeftControl)) tri-=100;
			colors[triangles[tri]] = Color.red;
			mesh.colors = colors;
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			colors[triangles[tri]] = Color.white;
			tri++;
			Debug.Log (triangles[tri] + "/" + (vertices.Length-1));
			if (Input.GetKey(KeyCode.LeftControl)) tri+=100;
			colors[triangles[tri]] = Color.red;
			mesh.colors = colors;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			triangles [tri]--;
			mesh.triangles = triangles;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			triangles [tri]++;
			mesh.triangles = triangles;
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			grabbed = !grabbed;
		}
		if (grabbed)
		{
			if (Input.GetButton("Fire1"))
			{
				grabbed = false;
			}
			vertices[triangles[tri]] += new Vector3 (Input.GetAxis("Mouse X")/3, Input.GetAxis("Mouse Y")/3, 0);
			mesh.vertices = vertices;
		}
	}
}
