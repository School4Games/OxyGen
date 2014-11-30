using UnityEngine;
using System.Collections;

public class Player
{
	//bool pressedButton = false;
	public int initialHealth = 3;
	public int currentHealth = 100;
	public int maxHealth = 50;

	public int initalWater = 50;
	public int currentWater = 100;

	// Reset currentWater and currentHealth before using them the first time, OR DIE!!!!!

	void Update () 
	{
		/*if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !pressedButton) 
		{
			transform.position += (Vector3.right*Input.GetAxis("Horizontal")).normalized;
			transform.position += (Vector3.up*Input.GetAxis("Vertical")).normalized;
			pressedButton = true;
		}
		else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) 
		{
			pressedButton = false;
		}*/
	}
	public void Reset()//resets Water and Health to initial Values
	{
		currentHealth = initialHealth;
		maxHealth = currentHealth;
		currentWater = initalWater;
	}		
}
