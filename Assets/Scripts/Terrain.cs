using UnityEngine;
using System.Collections;

[System.Serializable]
public class Terrain 
{

	public string name;

	public Texture2D texture;
	public Sprite dungeonScreen;
	public Sprite outdoorScreen;
	public Sprite dungeonEnemy;
	public Sprite outdoorEnemy;

	public ResourceEvent[] resourceEvents;
	public FightEvent[] fightEvents;
	public DungeonEvent[] dungeonEvents;
}
