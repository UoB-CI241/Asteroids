using UnityEngine;
using System.Collections;

public class HitByAsteroid : MonoBehaviour {

	//This attached to our ship and will handle it being hit by an asteroid
	public	AudioClip	ExplosionClip;		//Linked to sound clip in IDE
	Rigidbody2D	mRB;						//RB reference for faster use later
	void Start() {
		mRB = GetComponent<Rigidbody2D> ();	//Get RB refernece
	}
	//We are using trigger versions of the colliders as we don't want objects to bounce off eachother
	void OnTriggerEnter2D(Collider2D coll)		//Unity calls us when colliders intersect
	{
		if (coll.transform.parent) {			//This is due to the way the asteroids sprites are a child of the Asteroid game object 
			Asteroid tAsteroid = coll.transform.parent.gameObject.GetComponent<Asteroid> ();	//See if we really are an asteroid
			if (tAsteroid != null) {
				tAsteroid.Hit (mRB.velocity);		//Tell the Asteroid its been hit
				LevelManager.CreateSound(ExplosionClip);	//Explosion sound
				LevelManager.LooseLife ();					//We loose a life
			}
		}
	}
}



