using UnityEngine;
using System.Collections;

public class ExtMonoBehaviour : MonoBehaviour {

	void OnEnable () {
		EventManager.OnAITurn += OnTurn;
	}
	
	void OnDisable () {
		EventManager.OnAITurn -= OnTurn;
	}

	void OnTurn () {

	}
}
