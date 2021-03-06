﻿using UnityEngine;
using System.Collections;

public class DungeonState : MonoBehaviour, IDungeonMenuMessageTarget
{
	public GameState gamestate;

	public void OnLeave ()
	{
		gamestate.dungeoning = false;
		gamestate.switchState ();
	}

	public void OnProceed ()
	{
		gamestate.chooseEvent (gamestate.worldMap.tiles[(int)gamestate.player.position.x, (int)gamestate.player.position.x], 2);
	}
}
