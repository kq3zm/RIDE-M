using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdController : MonoBehaviour {

	public Sprite[] crowdSprites;

	public GameObject crowd;
	private SpriteRenderer spriteRenderer;

	public LevelManager levelManager;

	void Start() {
		spriteRenderer = crowd.GetComponent<SpriteRenderer> ();
	}

	void Update() {
		int playerCrowdHype = levelManager.GetCrowdHype ();
		int spriteIndex = playerCrowdHype / 10;

		spriteRenderer.sprite = crowdSprites [spriteIndex];
	}
}
