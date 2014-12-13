using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public float sensitivity = 5;
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton("Fire2"))
		{
			transform.position -= Vector3.right * Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
			transform.position -= Vector3.up * Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
		}
	}
}
