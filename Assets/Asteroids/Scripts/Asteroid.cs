using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
	public	GameObject[]			Sizes;		//Allow the sprite objects for asteroid sizes to be set in IDE
	public	AudioClip 				Explode;	//Explosion SFX	set in IDE
	private	int						mSize=-1;		//Sanity check, if size -1 something did not initialise
	private	SpriteRenderer			mSR;		//Cached references of key components
	private	Rigidbody2D				mRB;
	private	GameObject				mAsteroid;	//Cached reference to dynamically generated asteroid
	public	void	SetSize(int vSize) {		//This determines the type of asteroid to spawn
		if (vSize < Sizes.Length) {		//Sanity check
			mSize = vSize;			//Keep a copy so we can decide which stage is next & for score
			mAsteroid = GameObject.Instantiate<GameObject> (Sizes [mSize]);		//Grab the reveant Asteroid from array set up in IDE and spawn it
			mAsteroid.transform.SetParent (transform);			//Parent it to this gameobject
			mSR = mAsteroid.GetComponent<SpriteRenderer> ();	//Get the sprite renderer and RB, for faster access later
			mRB = mAsteroid.GetComponent<Rigidbody2D> ();
		}
	}
	public	Vector2 Position {					//Using a setter / getter here to keep it tidy, this makes a method act as it it was a variable
		set {
			mRB.transform.position = value;		//set position in RB vs transform as we are using physics
		}
		get {
			return mRB.transform.position;		//get position in RB vs transform as we are using physics
		}
	}
	public	Vector2	Velocity {			//Using a setter / getter here to keep it tidy, this makes a method act as it it was a variable	
		set {
			mRB.velocity = value;	//set velocity in RB vs transform as we are using physics
		} 
		get	{
			return	mRB.velocity;
		}
	}
	public void	Hit(Vector2 vDirection) {			//This lets out asteroid know its been hit
		float	vSpeed = vDirection.magnitude;		//Work out how fast, I use this later to make the splitting into more objects visually more appealing
		Quaternion	tOffset1 = Quaternion.Euler (0, 0, 45);		//Get 2 angles at right angle, this allows me to have the asteroid look like its splitting nicely
		Quaternion	tOffset2 = Quaternion.Euler (0, 0, -45);
		Quaternion	tAngle=Quaternion.Euler(0,0,Mathf.Atan2(vDirection.y,vDirection.x));		//Calculate the angle of the bullet, this is used to make the asteroid recoil
		Vector2 tSummedDirection = Velocity + vDirection;				//Simple calculation (not using mass) to work out which way the new rocks will fly off
		switch (mSize) {			//Depending on size make 2 or 3 new rocks
		case	0:
			LevelManager.CreateAsteroid (mSize + 1, Position, (tAngle * tOffset1) * tSummedDirection);	//Send them in different directions
			LevelManager.CreateAsteroid (mSize + 1, Position, (tAngle * tOffset2) * tSummedDirection);
			Destroy (gameObject);				//Destroy the one (this one) which was hit
			LevelManager.AddScore (100);			//Give score
			LevelManager.CreateSound (Explode);		//Make some noise
			break;

		case	1:
			LevelManager.CreateAsteroid (mSize+1, Position, (tAngle*tOffset1)*tSummedDirection);
			LevelManager.CreateAsteroid (mSize+1, Position, tAngle*tSummedDirection);
			LevelManager.CreateAsteroid (mSize+1, Position, (tAngle*tOffset2)*tSummedDirection);
			Destroy (gameObject);
			LevelManager.AddScore (200);
			LevelManager.CreateSound (Explode);
			break;

		case 2:									//At this stage there are no more asteroids to split into, so we just die
			Destroy (gameObject);
			LevelManager.AddScore (300);
			LevelManager.CreateSound (Explode);
			break;

		default:
			break;
		}
	}
	public	void	RandomPosition() {					//Find a random position in viewable space 
		float tHeight = Camera.main.orthographicSize;
		float tWidth = Camera.main.aspect * tHeight;
		do {
			mRB.transform.position = new Vector2 (Random.Range (-tWidth, tWidth), Random.Range (-tHeight, tHeight));	
		} while(mSR.bounds.Intersects(LevelManager.ShipBounds));		//Check we are not putting the asteroid on the player ship
	}

	void	Start() {
		if (mSize < 0) {
			Debug.Log ("Must call SetSize() to get correct Asteroid shape");		//Sanity check, to make sure SetSize was called, or it wont render anything
		}
		mRB.angularVelocity = Random.Range (-500f, 500f);			//Random spin
	}
}
