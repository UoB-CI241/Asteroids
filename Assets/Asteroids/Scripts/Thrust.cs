using UnityEngine;
using System.Collections;


public class Thrust : MonoBehaviour
{

    [Range(10f, 1000f)]					//Expose the max Thrust in IDE for easy editing
    public float MaxThrust = 100f;


    private Rigidbody2D mRB;		//Cached RigidBody
	private Animator	mAni;		//Cached Animator, this will allow us to change the animation to out the rocket plume on
	private	AudioSource	mAudio;		//Audio for thrust

    void Start() {
        mRB = GetComponent<Rigidbody2D>();		//Get all the required components
		mAni = GetComponent<Animator> ();
		mAudio = GetComponent<AudioSource> ();
    }

    void FixedUpdate() {
		if (Input.GetKey (KeyCode.DownArrow)) {
			Vector3 mForce = transform.rotation * Vector3.up; //Direction I am pointing in, using a unit up vector, rotated by my own rotation
			mRB.AddForce (mForce * MaxThrust);			//Add force in the direction of movement
			mAni.SetBool ("Thrust", true);				//Tell the animator to show the thrust anim
			if (!mAudio.isPlaying) {					//If not playing play rocket noise
				mAudio.Play ();
			}
		} else {
			mAni.SetBool ("Thrust", false);			//Turn of Thrust anim and audio
			if (mAudio.isPlaying) {
				mAudio.Stop ();
			}
		}
    }
}
