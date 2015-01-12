using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Image image in GetComponentsInChildren<Image>())
		{
			Debug.Log (image.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
