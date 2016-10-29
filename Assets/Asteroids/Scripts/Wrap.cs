using UnityEngine;
using System.Collections;

public class Wrap : MonoBehaviour {

	Rigidbody2D	mRB;					//Store out RB reference for faster access

	void	Start() {
		mRB = GetComponent<Rigidbody2D> ();					//We are going to use the rigidbody to reposition our GO
		//as suggested by the Unity API guide https://docs.unity3d.com/ScriptReference/Rigidbody.MovePosition.html
	}

	//Note as we are using Physics we will use the rigidbody.postition vs the transform one, as suggested by the Unity manual
	void FixedUpdate () {
        float tHeight = Camera.main.orthographicSize;		//We get the size of the viewable space here, starting with Height
        float tWidth = Camera.main.aspect*tHeight;			//Once we have the Height we can calculate the width using the aspect ratio

		if (mRB.position.x > tWidth) {						//These code sections check is we are off screen (right or left)
			mRB.position += Vector2.left * tWidth*2;		//This is a little trick to move us 2 x Width to the left
		} else if (mRB.position.x < -tWidth)				//Using an else here as we cant be both off the right & left
        {
			mRB.position += Vector2.right * tWidth*2;
        }

		if (mRB.position.y > tHeight)						//Same for height
        {
			mRB.position += Vector2.down * tHeight*2;
        }
		else if (mRB.position.y < -tHeight)
        {
			mRB.position += Vector2.up * tHeight*2;
        }
    }
}
