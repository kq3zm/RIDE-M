using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private float dragDistance;  //minimum distance for a swipe to be registered
	private float currentPosition = 0;

	private Vector3 rightPosition = new Vector3(.473f,.105f,-7.2f);
	private Vector3 centerPosition = new Vector3(0,.105f,-7.2f);
	private Vector3 leftPosition = new Vector3(-.473f,.105f,-7.2f);
	private float jumpHeight = 0.6f;
	private bool isGameOver = false;

	public int collectedNotes = 0;

	public float moveTime = 0.05f;
	public bool executedShift = false;
	public AudioClip booing;
	public AudioClip obstacleHit;
	public SoundManager soundManager;

	private LevelManager levelManager;

	public Behaviour halo;
	private int haloOffDelay;

	void Start()
	{
		dragDistance = Screen.width * 2 / 100; //dragDistance is 15% height of the screen
		levelManager = GetComponent<LevelManager>();
		halo = (Behaviour)GetComponent ("Halo");
		halo.enabled = false;
		haloOffDelay = 0;
	}

	void OnCollisionEnter (Collision other) {
		if (!isGameOver) {
			if (other.gameObject.CompareTag ("Note")) {
				levelManager.rep += levelManager.multiplier * 10;
				levelManager.SetRepText ();
				levelManager.IncreaseMultiplier ();
				if (!levelManager.isUltimateCrowdHype) {
					levelManager.IncreaseCrowdHype ();
					levelManager.IncreaseCO2 ();
				}
				levelManager.SetCo2Bar ();

				halo.enabled = true;

				Destroy (other.gameObject);
				collectedNotes += 1;
			}

			if (other.gameObject.CompareTag ("Obstacle")) {
				soundManager.PlayObstacleSound (obstacleHit);
				if (!levelManager.isUltimateCrowdHype) {
					levelManager.ResetMultiplier ();
					levelManager.DecreaseCrowdHype ();
				}
				Destroy (other.gameObject);
			}
		}
	}

	void Update()
	{
		if (levelManager.crowdHype <= 0 || isGameOver) {
			isGameOver = true;
			levelManager.GameOver ();
			if (!soundManager.isPlaying (booing))
				soundManager.PlayBooSound (booing);
		}  else {
			if (haloOffDelay == 10) {
				halo.enabled = false;
				haloOffDelay = 0;
			}

			if (Input.touchCount == 1) // user is touching the screen with a single touch
			{
				Touch touch = Input.GetTouch(0); // get the touch
				if (touch.phase == TouchPhase.Began) //check for the first touch
				{
					fp = touch.position;
					lp = touch.position;
				}
				else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
				{
					lp = touch.position;

					//Check if drag distance is greater than 20% of the screen height
					if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
					{//It's a drag
						//check if the drag is vertical or horizontal
						if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
						{     //If the horizontal movement is greater than the vertical movement...
							if ((lp.x > fp.x))  //If the movement was to the right)
							{     //Right swipe
								Debug.Log("Right Swipe");
								if (currentPosition == 0) {
									if (!executedShift) {
										currentPosition = 1;
										executedShift = true;
										iTween.MoveTo (gameObject, rightPosition, 0.2f);
									}
								}   else if (currentPosition == -1) {
									if (!executedShift) {
										currentPosition = 0;
										executedShift = true;
										iTween.MoveTo (gameObject, centerPosition, 0.3f);
									}
								}
							}
							else
							{     //Left swipe
								Debug.Log("Left Swipe");
								if (currentPosition == 0) {
									if (!executedShift) {
										currentPosition = -1;
										executedShift = true;
										iTween.MoveTo (gameObject, leftPosition, 0.2f);
									}
								}   else if (currentPosition == 1) {
									if (!executedShift) {
										currentPosition = 0;
										executedShift = true;
										iTween.MoveTo (gameObject, centerPosition, 0.2f);
									}
								}
							}
						}
						else
						{     //the vertical movement is greater than the horizontal movement
							if (!executedShift) {
								if (lp.y > fp.y) {  //If the movement was up//Up swipe
									Debug.Log ("Up Swipe");
									iTween.MoveTo (gameObject, iTween.Hash("position",new Vector3(gameObject.transform.position.x, jumpHeight, gameObject.transform.position.z),"easetype",iTween.EaseType.easeInQuad,"time",.3f, "onComplete", "movePlayerDown"));
									executedShift = true;
								}   else {   //Down swipe
									Debug.Log ("Down Swipe");
								}
							}
						}
					}
					else
					{     //It's a tap as the drag distance is less than 20% of the screen height
						Debug.Log("Tap");
					}
				}
				else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
				{
					lp = touch.position;  //last touch position. Ommitted if you use list
					executedShift = false;
				}
			}	
		}

		haloOffDelay++;
	}

	void movePlayerDown() {
		iTween.MoveTo (gameObject, iTween.Hash("position",new Vector3(gameObject.transform.position.x, 0.105f, gameObject.transform.position.z),"easetype",iTween.EaseType.easeInQuad,"time",.3f, "onComplete", "movePlayerDown"));
	}
}