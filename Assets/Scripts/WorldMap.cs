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

	private tile[,] tiles;

	public int width;
	public int height;

	public Material floors;
	public int floorRows;
	public Material structures;
	public int structureRows;
	public Material units;
	public int unitRows;

	GameObject floorLayer;
	GameObject structureLayer;
	GameObject unitLayer;

	public void generate () {
		tiles = new tile[width,height];
		for (uint y=0; y<height; y++) {
			for (uint x=0; x<width; x++) {
				tiles[x,y] = new tile();
			}
		}
		//do generation stuff
		for (uint y=0; y<height; y++) {
			for (uint x=0; x<height; x++) {

			}
		}
		instantiateGraphics ();
	}

	public void instantiateGraphics () {
		//create floor plane
		//for testing
		if (floorLayer != null) DestroyImmediate(floorLayer); 
		if (floorLayer == null) floorLayer = createPlane(width, height, 1, floors);
		//create structure plane
		//create unit plane
		//initiate 2D array
	}

	public GameObject createPlane (int xTiles, int yTiles, float tileSize, Material material) {
		GameObject plane = new GameObject();
		MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
		plane.AddComponent<MeshRenderer>();
		plane.renderer.material = material;

		meshFilter.sharedMesh = new Mesh();
		Mesh mesh = meshFilter.sharedMesh;

		Vector3[] vertices = new Vector3[xTiles * yTiles * 4];
		int[] triangles = new int[xTiles * yTiles * 6];
		Vector2[] uv = new Vector2[vertices.Length];

		string log = "";
		for (int y=0; y<height; y++) {
			for (int x=0; x<width; x++) {
				/*log += "x:" + x + "y:" + y + ", ";
				vertices[0] = new Vector3(0, 0, 0);
				vertices[1] = new Vector3(1, 0, 0);
				vertices[2] = new Vector3(0, 1, 0);
				vertices[3] = new Vector3(1, 1, 0);

				log += (x+(y*width-1))*4 + ", " + (x+(y*width-1)+1)*4 + ", " + (x+(y*width-1)+2)*4 + ", " + (x+(y*width-1)+3)*4 + ", ";
				
				triangles[0] = 0;
				triangles[1] = 2;
				triangles[2] = 1;
				triangles[3] = 1;
				triangles[4] = 2;
				triangles[5] = 3;*/
				//vertices confirmed to work
				vertices[x*4+y*width*4] = new Vector3(x, y, 0);
				vertices[x*4+1+y*width*4] = new Vector3((x+1), y, 0);
				vertices[x*4+2+y*width*4] = new Vector3(x, (y+1), 0);
				vertices[x*4+3+y*width*4] = new Vector3((x+1), (y+1), 0);

				if ((x*4+y*width*4) > 755) {
				log += (x*4+y*width*4) + ":" + new Vector3(x, y, 0) + ", "
					+ (x*4+1+y*width*4) + ":" + new Vector3((x+1), y, 0) + ", "
					+ (x*4+2+y*width*4) + ":" + new Vector3(x, (y+1), 0) + ", "
					+ (x*4+3+y*width*4) + ":" + new Vector3((x+1), (y+1), 0) + ", "
					+ "\n";
				}

				triangles[x*6+y*width*6] = x*4+y*width;
				triangles[x*6+1+y*width*6] = x*4+2+y*width;
				triangles[x*6+2+y*width*6] = x*4+1+y*width;
				triangles[x*6+3+y*width*6] = x*4+1+y*width;
				triangles[x*6+4+y*width*6] = x*4+2+y*width;
				triangles[x*6+5+y*width*6] = x*4+3+y*width;

				uv[x*4+y*width] = new Vector2(0, 0);
				uv[x*4+1+y*width] = new Vector2(1, 0);
				uv[x*4+2+y*width] = new Vector2(0, 1);
				uv[x*4+3+y*width] = new Vector2(1, 1);
			}
		}

		Debug.Log(log);

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();

		plane.name = material.name;
		return plane;
	}
}
