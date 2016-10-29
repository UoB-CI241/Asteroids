using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {

	[Range(200f,500f)]
	public	float	Speed=200f;		//Degrees per second

    private Rigidbody2D mRB;

	// Use this for initialization
	void Start () {
        mRB = GetComponent<Rigidbody2D>();  //Get reference to Rigid Body 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			mRB.angularVelocity = Speed;
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			mRB.angularVelocity = -Speed;
		} else {
			mRB.angularVelocity = 0f;
		}
    }
}
