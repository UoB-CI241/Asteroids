using UnityEngine;
using System.Collections;

public class MaxSpeed : MonoBehaviour {

    [Range(1f, 100f)]				//Expose speed govenor in IDE
    public float Speed = 1f;

    private Rigidbody2D mRB;		//RB cached for faster access

    // Use this for initialization
    void Start () {
        mRB = GetComponent<Rigidbody2D>();		//Grab RB reference
    }
	// FixedUpdate is called once per Physics frame, you should always interact with RB's in FixedUpdate and not Update()
	void FixedUpdate () {
        if (mRB.velocity.magnitude > Speed)		//Are we breaking the speed limit?
        {
            mRB.velocity *= Speed / mRB.velocity.magnitude;		//Slow down
        }
    }
}



