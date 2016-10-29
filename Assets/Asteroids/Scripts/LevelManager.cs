using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class LevelManager : MonoBehaviour {

	public	Text	ScoreText;			//Variables set in IDE to link to UI
	public	Text	GameOverText;
	public	Image	Life1;
	public	Image	Life2;
	public	Image	Life3;


	public GameObject Ship;			//IDE links to prefabs used for ship & asteroid
    public GameObject Asteroid;

    [Range(5,30)]					//Expose speed for inital asteroids in IDE
    public float MaxSpeed;

	[Range(1,30)]					//How many asteroids to we have to start with, shown in IDE
	public int Asteroids;

	private	GameObject		mShip;			//Keep a reference to the ship instance


	private	int			mScore;			//Score and lives are maintained in Level manager
	private	int			mLives;

	public	static	LevelManager	LM;		//This is used to create a singleton object, suggest you search: singleton unity3d
	void	Awake() {			//This is used to create a singleton object, suggest you search: singleton unity3d
		if (LM == null) {			//Its a nifty trick to allow me to access key methods as if they were static
			LM = this;					//This is very useful as I can use it to have other method tell the level manager to update the score, lies and even create new asteroids
			DontDestroyOnLoad (LM.gameObject);		//Tell Unity not to delete this when the scene is deleted
		} else if (LM != this) {			//Soem house keeping to ensure there is only one of these
			Destroy (this);
		}
	}
	static	GameObject	AsteroidPrefab { //Use singleton to allow static setters & getters
		get {
			return	LM.Asteroid;			//Get Asteroid prefab
		}
	}
	public	static	Bounds	ShipBounds {	//Get Ship bounds, to check its safe
		get {
			return	LM.mShip.GetComponent<SpriteRenderer> ().bounds;
		}
	}

	void	NewGame() {		//Clear gameobjects, wait 2 seconds and start the game
		Clear ();
		Invoke ("Restart", 2f);		//Delayed start
	}
	void	Clear() {		//Clear all generated objects
		GameObject[] 	tToDestroy=GameObject.FindGameObjectsWithTag (DisposableTag);		//I tag all dynamic objects with this so its easy to clear them
		if (tToDestroy != null) {		//Get an array of objects to destroy
			foreach (GameObject tGO in tToDestroy) {	//Step through and destroy them
				Destroy (tGO);
			}
		}
		mScore = 0;			//Reset game
		mLives = 3;
		ShowLives ();
		GameOverText.color = Color.green;	//Show get Ready Text
		GameOverText.text = "Get Ready";
		GameOverText.enabled = true;
		AddScore (0);			//Display score by adding 0
	}
	void Restart() {
		GameOverText.enabled = false;		//Remove GameOver/Ready banner
		MakeShip ();						//Make player ship
		for (int tI = 0; tI < Asteroids; tI++) {
			CreateInitialAsteroids ();		//make Asterroids
		}
	}
	// Use this for initialization
	void Start () {
		NewGame ();			//Set up first game
    }

	//Make a specific size asteroid, at a position with a set velocity
	public	static	void	CreateAsteroid(int vSize,Vector2 vPosition, Vector2 vVelocity) {
		GameObject tGOAsteroid=GameObject.Instantiate<GameObject>(AsteroidPrefab);
		Asteroid	tAsteroid=tGOAsteroid.GetComponent<Asteroid>();
		tGOAsteroid.tag = DisposableTag;
		tAsteroid.SetSize (vSize);		//Create correct type
		tAsteroid.Position=vPosition;		//This uses the setters found in asteroid.cs
		tAsteroid.Velocity = vVelocity;
	}


	public	static	void	CreateInitialAsteroids() {
		GameObject tGOAsteroid=GameObject.Instantiate<GameObject>(AsteroidPrefab);
		Asteroid	tAsteroid=tGOAsteroid.GetComponent<Asteroid>();
		tGOAsteroid.tag = DisposableTag;
		tAsteroid.SetSize (0);		//Start With Big ones
		tAsteroid.RandomPosition();
		tAsteroid.Velocity = new Vector2(Random.Range(-LM.MaxSpeed, LM.MaxSpeed), Random.Range(-LM.MaxSpeed, LM.MaxSpeed));
	}

	void	MakeShip() {
		mShip=Instantiate<GameObject>(Ship);
		mShip.tag=DisposableTag;
	}

	static	public	void	AddScore(int vScore) {
		LM.mScore += vScore;
		LM.ScoreText.text = string.Format ("Score {0}", LM.mScore);
	}

	//Use UI to show how many lives are left
	void	ShowLives() {
		switch (mLives) {
		case	0:
			Life1.enabled = false;		//Turn on/off the images in UI to show ship sprites
			Life2.enabled = false;
			Life3.enabled = false;
			break;

		case	1:
			Life1.enabled = true;
			Life2.enabled = false;
			Life3.enabled = false;
			break;

		case	2:
			Life1.enabled = true;
			Life2.enabled = true;
			Life3.enabled = false;
			break;

		case	3:
			Life1.enabled = true;
			Life2.enabled = true;
			Life3.enabled = true;
			break;
		default:
			break;
		}
	}

	//Use static call to reduce lives and trigger new ship or game over
	static	public	void	LooseLife () {
		if (LM.mLives > 0) {		//If we have lives, keep playing
			LM.mLives--;
			Destroy (LM.mShip);		//Destroy current ship
			LM.mShip = null;
			LM.Invoke ("MakeShip", 2f);		//Make new one in 2 seconds
		} else {						//If game over
			Destroy (LM.mShip);			//Destroy ship
			LM.mShip = null;
			LM.GameOverText.color = Color.red;		//Show Game over text
			LM.GameOverText.text="Game Over";		
			LM.GameOverText.enabled = true;
			LM.Invoke ("NewGame", 7f);		//Trigger new game after 7 seconds
		}
		LM.ShowLives ();		//Make sure right number of lives are showing
	}


	//Static method to create a soundclip attached to a GameObject
	static	public	void	CreateSound(AudioClip vClip) {
		GameObject	tSound = new GameObject ();
		AudioSource	tSource=tSound.AddComponent<AudioSource> ();		//Make GO and add AudioSource to play clip
		tSource.name = vClip.name;				//Give it a useful name in IDE
		tSource.clip = vClip;					//Clip to play
		tSound.tag = DisposableTag;			//This Tag is used to make sure I can clear all the tagged objects on game restart
		tSource.Play ();						//play it sam
		Destroy (tSound, vClip.length);			//Auto destruct clip when its finished playing
	}

	static	public	string	DisposableTag {
		get {
			return	"DeleteOnRestart";
		}
	}

}
