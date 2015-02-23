using UnityEngine;
using System.Collections;

public class MenuState : MonoBehaviour, IMenuMessageTarget 
{
	#region IMenuMessageTarget implementation

	public void OnStart ()
	{
		Application.LoadLevel (1);
	}

	#endregion
}
