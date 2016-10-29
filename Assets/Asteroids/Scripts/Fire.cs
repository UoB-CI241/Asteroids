using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
	[Range(0.25f,20f)]				//Expose fire rate in IDE for easy edit
	public	float	Rate;

	[Range(1f, 100f)]				//Expose Bullet speed in IDE for easy edit
	public 	float 	Speed;

    public GameObject Bullet;		//The bullet sprite GO, linked in IDE

	public	AudioClip 				FireSound;		//Fire sound clip, linked in IDE

	private	float	mCoolDown;		//Fire rate cooldown, 1/Rate

	void	Start() {
		mCoolDown = 0.0f;		//Reminder it should be zero
	}
    void Update()
    {
		if (mCoolDown <= 0f) {			//Only allow fire if we have cooled down
			if (Input.GetKey (KeyCode.Space)) {		//Fire Key pressed?
				mCoolDown = 1f / Rate;		//Calculate new cooldown
				Vector2 tDirection = transform.rotation * Vector2.up;		//To fire we need to know which way we are pointing
				GameObject tBullet = Instantiate<GameObject> (Bullet);		//make new bullet using object set in IDE
				tBullet.tag=LevelManager.DisposableTag;
				Rigidbody2D tRB = tBullet.GetComponent<Rigidbody2D> ();		//Get RB as we are using physics
				tRB.transform.position = (Vector2)transform.position + tDirection;	//Place bullet at ship origin + the rotated fire direction vector
				tRB.velocity = tDirection * Speed;					//Set bullet speed
				LevelManager.CreateSound (FireSound);				//Make fire noise
			}
		} else {
			mCoolDown -= Time.deltaTime;		//process cooldown
		}
    }
}



