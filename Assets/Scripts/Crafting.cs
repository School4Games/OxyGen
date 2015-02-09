using UnityEngine;
using System.Collections;

public class Crafting : MonoBehaviour, ICraftingMenuMessageTarget {

	public Player player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region ICraftingMenuMessageTarget implementation

	public void OnCraftMedipack ()
	{
		player.inventory.addResource(Resource.Type.Health, 1);
		player.inventory.addResource(Resource.Type.Scrap, -5);
	}

	public void OnCraftArmor ()
	{
		throw new System.NotImplementedException ();
	}

	public void OnCraftInventorySlot ()
	{
		player.inventory.updateSlotNumber(player.inventory.slotNumber+1);
		player.inventory.addResource(Resource.Type.Scrap, -5);
	}

	public void OnCraftVisionUpgrade ()
	{
		throw new System.NotImplementedException ();
	}

	public void OnCraftpermashield ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
