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
		player.inventory.addResource(Resource.Type.Scrap, -5);
		player.inventory.addResource(Resource.Type.Health, 1);
	}

	public void OnCraftInventorySlot ()
	{
		player.inventory.addResource(Resource.Type.Scrap, -10);
		player.inventory.updateSlotNumber(player.inventory.slotNumber+1);
	}

	public void OnCraftVisionUpgrade ()
	{
		player.inventory.addResource(Resource.Type.Scrap, -10);
		fogOfWar.gameObject.transform.position += Vector3.forward;
	}

	public void OnCraftpermashield ()
	{
		player.inventory.addResource(Resource.Type.Scrap, -10);
		player.inventory.addResource(Resource.Type.Armor, 1);
	}

	#endregion
}
