using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	void OnEnable () {
		EventManager.OnAITurn += OnTurn;
	}

	void OnDisable () {
		EventManager.OnAITurn -= OnTurn;
	}

	void OnTurn (Vector3 position) {
		Vector3 toPlayer = position -= transform.position;
		if (Mathf.Abs(toPlayer.x) >= Mathf.Abs(toPlayer.y)) {
			transform.position += Vector3.right * Mathf.Sign(toPlayer.x);
		}
		else {
			transform.position += Vector3.up * Mathf.Sign(toPlayer.y);
		}
	}
}
