using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderTest : MonoBehaviour 
{

	Slider slider;

	// Use this for initialization
	void Start () 
	{
		slider = GetComponent<Slider> ();
		slider.maxValue = 15;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.R))
		{
			slider.maxValue = Random.Range (1, 30);
		}
	}
}
