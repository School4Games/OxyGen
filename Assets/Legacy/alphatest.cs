using UnityEngine;
using System.Collections;

public class alphatest : MonoBehaviour {

	Texture2D main;

	Color[] stamp = new Color[100];

	// Use this for initialization
	void Start () {
		main = (Texture2D)renderer.material.mainTexture;
		for(int i=0; i<stamp.Length; i++) {
			stamp[i] = new Color(0,0,0,0);
		}
		main.SetPixels(Random.Range(0, main.width-5), Random.Range(0, main.height-5), 10, 10, stamp);
		main.Apply();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
