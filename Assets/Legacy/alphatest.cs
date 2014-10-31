using UnityEngine;
using System.Collections;

public class alphatest : MonoBehaviour {

	Texture2D main;

	public Texture2D stamp;
	public int hexSideLength = 64;

	//Color[] stamp = new Color[100];

	// Use this for initialization
	void Start () {
		if (stamp != null) {
			main = (Texture2D)renderer.material.mainTexture;
			Color[] colors = new Color[stamp.height * stamp.width];
			Color[] oldColors = new Color[stamp.height * stamp.width];
			/*for(int y=0; y<stamp.height; y++) {
				for(int x=0; x<stamp.width; x++) {

				}
				//stamp[i] = new Color(0,0,0,0);
			}*/
			colors = stamp.GetPixels(0, 0, stamp.width, stamp.height);
			int x = Random.Range(0, main.width-hexSideLength);
			int y = Random.Range(0, main.width-hexSideLength);
			oldColors = main.GetPixels(x, y, stamp.width, stamp.height);
			string log = "";
			for (int i=0; i<colors.Length; i++) {
				oldColors[i].a = colors[i].grayscale; 
			}
			Debug.Log(log);
			main.SetPixels(x, y, stamp.width, stamp.height, oldColors);
			main.Apply();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
