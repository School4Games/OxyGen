using UnityEngine;
using System.Collections;

public class Crafting : MonoBehaviour, ICraftingMenuMessageTarget {

	public Player player;

	public ShaderFogOfWar fogOfWar;

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

	public void OnCraftInventorySlot ()
	{
		player.inventory.updateSlotNumber(player.inventory.slotNumber+1);
		player.inventory.addResource(Resource.Type.Scrap, -10);
	}

	public void OnCraftVisionUpgrade ()
	{
		fogOfWar.gameObject.transform.position += Vector3.forward;
		player.inventory.addResource(Resource.Type.Scrap, -10);
	}

	public void OnCraftpermashield ()
	{
		player.inventory.addResource(Resource.Type.Armor, 1);
		player.inventory.addResource(Resource.Type.Scrap, -10);
	}

	#endregion
}
