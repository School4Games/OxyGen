using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ResourceParticles : MonoBehaviour 
{
	ParticleSystem particleSystem;

	Camera cam;

	float lifetime;

	float age;

	Vector3 startPosition;

	void Start () 
	{
		cam = Camera.main;

		startPosition = transform.position;

		lifetime = particleSystem.startLifetime;
	}

	// Update is called once per frame
	void Update () 
	{
		age += Time.deltaTime;
		goToScreenCenter ();
	}

	public void emit (int amount)
	{
		particleSystem = GetComponent ("ParticleSystem") as ParticleSystem;
		particleSystem.maxParticles = amount;
		particleSystem.Play ();
	}

	void goToScreenCenter ()
	{
		if (age < lifetime)
		{
			transform.position = cam.ScreenToWorldPoint (new Vector3 (Screen.width/2, 0)) + (startPosition - cam.ScreenToWorldPoint (new Vector3 (Screen.width/2, 0)) * ((lifetime-age)/lifetime));
		}
		else
		{
			Destroy (this.gameObject);
		}
	}
}
