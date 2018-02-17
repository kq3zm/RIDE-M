using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	// UI
	public Text repText;
	public Text multiplierText;
	public GameObject co2SmokeL;
	public GameObject co2SmokeR;

	// Modal Screen
	public GameObject modalCanvasGameObject;
	public Text titleText;
	public Text scoreText;
	public Text percentText;
	public Text notesCountText;
	public Button mainMenuButton;
	public Button retryButton;

	// Level Data
	public int rep;
	public int multiplier;
	public int crowdHype;
	public int co2;
	public Slider Co2Meter;
	public Button activateCo2;
	float timeRemaining = 5;
	public bool isUltimateCrowdHype;

	// Component References
	public SoundManager soundManager;
	public NoteController noteController;
	public PlayerController playerController;
	private AudioClip music;
	public AudioClip booing;
	public AudioClip cheering; 
	public AudioClip co2Sound;
	public AudioClip buttonSound;

	// Use this for initialization
	void Start () {
		rep = 0;
		multiplier = 1;
		SetRepText ();
		multiplierText.text = "";

		crowdHype = 60; // Out of 100 total (abstracted from user)
		co2 = 0; // Out of 15 total 
		SetCo2Bar();

		co2SmokeL = GameObject.Find ("WhiteSmokeL");
		co2SmokeR = GameObject.Find ("WhiteSmokeR");
		co2SmokeL.SetActive (false);
		co2SmokeR.SetActive (false);

		Button co2button = activateCo2.GetComponent<Button> ();
		co2button.enabled = false;
		co2button.gameObject.SetActive (false);
		co2button.onClick.AddListener (ActivateCo2);
		isUltimateCrowdHype = false;

		Button mainMenuButtonCR = mainMenuButton.GetComponent<Button>();
		mainMenuButtonCR.onClick.AddListener(segueMainMenu);

		Button retryButtonCR = retryButton.GetComponent<Button>();
		retryButtonCR.onClick.AddListener(resetScene);

		modalCanvasGameObject = GameObject.Find ("ModalCanvas");
		modalCanvasGameObject.SetActive(false);

		switch (GameManager.difficulty) {
		case Difficulty.easy:
			music = Resources.Load ("Audio/easy") as AudioClip;
			print ("loaded easy");
			break;
		case Difficulty.medium:
			music = Resources.Load("Audio/medium") as AudioClip;
			print ("loaded medium");
			break;
		case Difficulty.hard:
			music = Resources.Load ("Audio/hard") as AudioClip;
			print ("loaded hard");
			break;
		}

		print (music);

		soundManager.PlaySingle (music);
	}
	
	// Update is called once per frame
	void Update () {
		if (isUltimateCrowdHype) {
			timeRemaining -= Time.deltaTime;

			// 32x multiplier
			multiplier = 32;
			SetMultiplierText ();

			// Maxed out crowd hype
			crowdHype = 100;

			// 5 seconds are up
			if (timeRemaining <= 0) {
				// u.c.h. now over
				isUltimateCrowdHype = false;

				// turn off smoke
				co2SmokeL.SetActive(false);
				co2SmokeR.SetActive(false);

				// reset multiplier
				multiplier = 16;
				SetMultiplierText ();

				// reset timeRemaining for next time u.c.h. happens
				timeRemaining = 5;
			}
		}
		// Level win condition
		if (!soundManager.efxSource.isPlaying && crowdHype > 0)
		{
			LevelBeaten ();
			if (!soundManager.isPlaying (cheering))
				soundManager.PlayCheerSound (cheering);
		}


		if (crowdHype < 20) {
			if (!soundManager.isPlaying (booing))
				soundManager.PlayBooSound (booing);
		}
	}

	void resetScene() {
		soundManager.PlayButtonSound (buttonSound);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void segueMainMenu() {
		soundManager.PlayButtonSound (buttonSound);
		SceneManager.LoadScene("Intro");
	}

	public void IncreaseMultiplier () {
		if (multiplier < 16) {
			multiplier *= 2;
		}
		SetMultiplierText ();
	}

	public void ResetMultiplier () {
		multiplier = 1;
		multiplierText.text = "";
	}

	void SetMultiplierText() {
		multiplierText.text = multiplier.ToString() + "x";
	}

	public void IncreaseCrowdHype() {
		crowdHype += 5;
		if (crowdHype > 100)
			crowdHype = 100;
	}

	public void DecreaseCrowdHype() {
		crowdHype -= 7;
		if (crowdHype < 0)
			crowdHype = 0;
	}

	public int GetCrowdHype() {
		return crowdHype;
	}

	public void IncreaseCO2() {
		if (!isUltimateCrowdHype && multiplier >= 16 && co2 < 15) {
			co2++;
		}
	}

	public void SetCo2Bar() {
		Co2Meter.value = co2;
		if (co2 >= 15) {
			Button co2button = activateCo2.GetComponent<Button> ();
			co2button.enabled = true;
			co2button.gameObject.SetActive (true);
		}
	}

	public void ActivateCo2() {
		co2 = 0;

		co2SmokeL.SetActive (true); // turn on smoke!!
		co2SmokeR.SetActive (true); // turn on smoke!!

		// Deactivate button
		Button co2button = activateCo2.GetComponent<Button> ();
		co2button.enabled = false;
		co2button.gameObject.SetActive (false);

		// toggle u.c.h on, so other controllers know that the player is in this state too
		isUltimateCrowdHype = true;

		// cannon sound
		soundManager.PlayCheerSound (cheering);
		soundManager.PlayCO2Sound (co2Sound);
	}

	public void SetRepText ()
	{
		repText.text = rep.ToString ();
	}

	public void GameOver() {
		// End game
		soundManager.StopSingle ();
		noteController.generateNotes = false;

		// Setup Game Over UI
		modalCanvasGameObject.SetActive(true);
		titleText.text = "Game Over";
		scoreText.text = rep.ToString ();
		percentText.text = ((int)((float)playerController.collectedNotes*100 / (float)noteController.totalNotes)).ToString () + " PERCENT";
		notesCountText.text = playerController.collectedNotes.ToString () + " NOTES";
	}

	public void LevelBeaten() {
		// Setup Game Over UI
		modalCanvasGameObject.SetActive(true);
		titleText.text = "Fantastic!";
		scoreText.text = rep.ToString ();
		percentText.text = ((int)((float)playerController.collectedNotes*100 / (float)noteController.totalNotes)).ToString () + " PERCENT";
		notesCountText.text = playerController.collectedNotes.ToString () + " NOTES";
	}
}
