using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

	public LevelManager levelManager;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Note")) {
			if (!levelManager.isUltimateCrowdHype) {	
				levelManager.ResetMultiplier ();
				levelManager.DecreaseCrowdHype ();
			}
			Destroy (other.gameObject);
		}
		if (other.gameObject.CompareTag ("Obstacle")) {
			Destroy (other.gameObject);
		}

	}
}