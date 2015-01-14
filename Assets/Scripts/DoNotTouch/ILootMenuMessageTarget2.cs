﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public interface ILootMenuMessageTarget : IEventSystemHandler
{
	// functions that can be called via the messaging system
	void OnLeave();
}
