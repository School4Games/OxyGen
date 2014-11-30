using UnityEngine;
using System.Collections;

public class Upgrades
{
	Player player;

	public int healthPackCount = 0; //How many Healthpaclthe player has
	public int healthUpgradeCount = 0; //How many Health Upgrades the player has

	public void FindHealthPack () //Player finds a new healthpack
	{
		healthPackCount++;
	}

	public void UseHealthpack() //Player uses a new Healthpack. It heals for 1 health if the health is below max Health
	{
		if (player.currentHealth < player.maxHealth && healthPackCount > 0)
		{
			player.currentHealth++;
			healthPackCount--;
		}
		else
		{
			Debug.Log ("That guy tried to heal himself with full health. Really?");
		}
	}

	public void FindHealthUpgrade() //Finds Health Upgrade
	{
			healthUpgradeCount++;
	}

	public void UseHealthUpgrade() //If the player uses the
	{
		if (healthUpgradeCount > 0)
		{
			player.maxHealth++;
			healthUpgradeCount--;
		}
	}

	public void ResetUpgrades()
	{
			healthPackCount = 0;
			healthUpgradeCount = 0;
	}

}
