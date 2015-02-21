using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShaderFogOfWar : MonoBehaviour 
{
	public Color clearColor;

	public Color highlightColor;

	public Vector2 windDriection = Vector2.one;
	public float cloudSpeed = 1;

	Mesh mesh;

	int mapWidth;
	int mapHeight;

	int highlightedTile = -1;

	// test
	int tri;
	bool grabbed = false;

	void Start () 
	{

	}

	void Update () 
	{
		if (mesh != null)
		{
			moveTexture ();
			testStuff ();
		}
	}

	public void createFogMesh (Vector2[,] positions)
	{
		mesh = GetComponent<MeshFilter> ().mesh;
		mapWidth = positions.GetUpperBound (0)+1;
		mapHeight = positions.GetUpperBound (1)+1;

		Vector3[] vertices = new Vector3[mapWidth*mapHeight+400];
		int [] triangles = new int[vertices.Length*18+24];
		Vector2[] uv = new Vector2[vertices.Length];
		int index = 0;
		int triIndex = 0;
		for (int y=0; y<mapHeight; y++)
		{
			for (int x=0; x<mapWidth; x++)
			{
				Vector3 position = new Vector3 (positions[x,y].x, positions[x,y].y, 0);
				// could do the same here as with triIndex
				vertices[index] = position;
				uv[index] = new Vector2 ((float)x/(float)mapWidth, (float)y/(float)mapHeight);

				if (x>0 && y<mapHeight-1)
				{
					// tri 1
					triangles[triIndex] = index;
					triIndex ++;
					// up
					triangles[triIndex] = index+mapWidth;
					triIndex ++;
					// even, upper left
					if (x % 2 == 0)
					{
						triangles[triIndex] = index-1;
						triIndex++;
					}
					// odd, upper left
					else
					{
						triangles[triIndex] = index-1+mapWidth;
						triIndex++;
					}
				}

				if (x>0 && (y>0 || x % 2 != 0) && (y<mapHeight-1 || x % 2 == 0))
				{
					// tri 2
					triangles[triIndex] = index;
					triIndex++;
					// even, upper left
					if (x % 2 == 0)
					{
						triangles[triIndex] = index-1;
						triIndex++;
					}
					// odd, upper left
					else
					{
						triangles[triIndex] = index-1+mapWidth;
						triIndex++;
					}
					// even, lower left
					if (x % 2 == 0)
					{
						triangles[triIndex] = index-1-mapWidth;
						triIndex++;
					}
					// odd, lower left
					else
					{
						triangles[triIndex] = index-1;
						triIndex++;
					}
				}
				index++;
			}
		}

		// test
		Color[] colors = new Color[vertices.Length];
		for (int i=0; i<colors.Length; i++)
		{
			colors[i] = Color.white;
			colors[i].a = 1;
			if (i%mapWidth != 0 && i%mapWidth != 1 && i>mapWidth*2 && i<colors.Length-mapWidth*2-4 && (i+1)%mapWidth != 0)
			{
				colors[i].r = Random.Range(0.5f, 1.0f);
				colors[i].g = colors[i].r;
				colors[i].b = colors[i].r;
			}
		}

		// create border
		// bottom
		float s = Vector3.Distance (vertices[0], vertices[1]);
		float h = Mathf.Pow (3.0f,0.5f)/2*s;
		for (int x=0; x<mapWidth; x++)
		{
			int v0 = x;
			int v1 = 0;
			int v2 = 0;
			int v3 = 0;
			Vector3 position = new Vector3 (positions[x,0].x, positions[x,0].y, 0);
			// create/ select vertex to the lower left
			if (x % 2 == 0)
			{
				vertices[index] = position + new Vector3 (-h,-s/2,0);
				v1 = index;
				colors[index] = Color.black;
				index ++;
			}
			else 
			{
				v1 = x-1;
			}
			// create vertex below
			vertices[index] = position + new Vector3 (0,-s,0);
			v2 = index;
			colors[index] = Color.black;
			index ++;
			// create/ select vertex to the lower right 
			if (x % 2 == 0 || x>=mapWidth-1)
			{
				vertices[index] = position + new Vector3 (h,-s/2,0);
				v3 = index;
				colors[index] = Color.black;
				index ++;
			}
			else
			{
				v3 = x+1;
			}
			
			// connect
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v1;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			triangles[triIndex] = v3;
			triIndex++;
		}

		// left
		for (int y=0; y<mapHeight; y++)
		{
			int v0 = y*mapWidth;
			int v1 = 0;
			int v2 = 0;
			int v3 = 0;
			Vector3 position = new Vector3 (positions[0,y].x, positions[0,y].y, 0);
			// create/ select vertex above
			if (y>=mapHeight-1)
			{
				vertices[index] = position + new Vector3 (0,s,0);
				v1 = index;
				colors[index] = Color.black;
				index ++;
			}
			else 
			{
				v1 = (y+1)*mapWidth;
			}
			// create vertex to the upper left
			vertices[index] = position + new Vector3 (-h,s/2,0);
			v2 = index;
			colors[index] = Color.black;
			index ++;
			// create vertex to the lower left
			vertices[index] = position + new Vector3 (-h,-s/2,0);
			v3 = index;
			colors[index] = Color.black;
			index ++;
			
			// connect
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v1;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			triangles[triIndex] = v3;
			triIndex++;
		}

		// top
		for (int x=0; x<mapWidth; x++)
		{
			int v0 = x+(mapHeight-1)*mapWidth;
			Debug.Log (v0);
			int v1 = 0;
			int v2 = 0;
			int v3 = 0;
			Vector3 position = new Vector3 (positions[x,mapHeight-1].x, positions[x,mapHeight-1].y, 0);
			// create/ select vertex to the upper right 
			if (x % 2 != 0 || x>=mapWidth-1)
			{
				vertices[index] = position + new Vector3 (h,s/2,0);
				v1 = index;
				colors[index] = Color.black;
				index ++;
			}
			else
			{
				v1 = v0+1;
			}
			// create vertex above
			vertices[index] = position + new Vector3 (0,s,0);
			v2 = index;
			colors[index] = Color.black;
			index ++;
			// create/ select vertex to the upper left
			if (x % 2 != 0)
			{
				vertices[index] = position + new Vector3 (-h,s/2,0);
				v3 = index;
				colors[index] = Color.black;
				index ++;
			}
			else 
			{
				v3 = v0-1;
			}
			
			// connect
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v1;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			
			if (x>0)
			{
				triangles[triIndex] = v0;
				triIndex++;
				triangles[triIndex] = v2;
				triIndex++;
				triangles[triIndex] = v3;
				triIndex++;
			}
		}

		// right
		for (int y=0; y<mapHeight; y++)
		{
			int v0 = y*mapWidth+(mapWidth-1);
			int v1 = 0;
			int v2 = 0;
			int v3 = 0;
			Vector3 position = new Vector3 (positions[mapWidth-1,y].x, positions[mapWidth-1,y].y, 0);
			// create vertex to the lower right
			vertices[index] = position + new Vector3 (h,-s/2,0);
			v1 = index;
			colors[index] = Color.black;
			index ++;
			// create vertex to the upper right
			vertices[index] = position + new Vector3 (h,s/2,0);
			v2 = index;
			colors[index] = Color.black;
			index ++;
			// create/ select vertex above
			if (y>=mapHeight-1)
			{
				vertices[index] = position + new Vector3 (0,s,0);
				v3 = index;
				colors[index] = Color.black;
				index ++;
			}
			else 
			{
				v3 = (y+1)*mapWidth+(mapWidth-1);
			}
			
			// connect
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v1;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			
			triangles[triIndex] = v0;
			triIndex++;
			triangles[triIndex] = v2;
			triIndex++;
			triangles[triIndex] = v3;
			triIndex++;
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

		if (x<0 || y<0 || x>mapWidth-1 || y>mapHeight-1)
		{
			return;
		}

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

	void moveTexture ()
	{
		Vector2 offset = windDriection * Time.time/30 * cloudSpeed;
		renderer.material.SetTextureOffset ("_MainTex", offset);
	}

	void testStuff ()
	{
		// test
		Color[] colors = mesh.colors;
		int[] triangles = mesh.triangles;
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
