using UnityEngine;
using System.Collections;

public class BulletHit : MonoBehaviour {
    public float TimeToLive = 2f;		//Expose bullet life time in IDE, so easy edit
	Rigidbody2D mRB;		//Cached RB
	void	Start() {
		mRB = GetComponent<Rigidbody2D> ();		//Get our RB
	}
	//Time out the bullet
    void Update() {
    TimeToLive -= Time.deltaTime;		//Reduce life
        if (TimeToLive <= 0f)
        {
            Destroy(gameObject);		//Time to Die
        }
    }
	//Unity calls this if the bullet hits something
    void OnTriggerEnter2D(Collider2D coll)
    {
		if (coll.transform.parent) {		//To allow for multiple Asteroids sizes, the sprite is actually a child of a fake GO
			Asteroid tAsteroid = coll.transform.parent.gameObject.GetComponent<Asteroid> ();	//Get the asteroid script
			if (tAsteroid != null) {
				tAsteroid.Hit (mRB.velocity);		//Tell Asteroid its been hit
				Destroy (gameObject);		//Bullet dies too
			}
		}
    }
}
