using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour
{
	private Vector3 position;
	private float width;
	private float height;

	void Awake()
	{
		width = (float)Screen.width / 2.0f;
		height = (float)Screen.height / 2.0f;

		// Position used for the cube.
		position = new Vector3(0.0f, 0.0f, 0.0f);
	}

	void OnGUI()
	{
		// Compute a fontSize based on the size of the screen width.
		GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

		GUI.Label(new Rect(20, 20, width, height * 0.25f),
			"x = " + position.x.ToString("f2") +
			", y = " + position.y.ToString("f2"));
	}

	void Update()
	{
		//// Handle screen touches.
		//if (Input.touchCount > 0)
		//{
		//	Touch touch = Input.GetTouch(0);

		//	//if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		//	//{

		//		// Move the cube if the screen has the finger moving.
		//		if (touch.phase == TouchPhase.Moved)
		//	{
		//		Vector2 pos = touch.position;
		//		//pos.x = (pos.x - width) / width;
		//		//pos.y = (pos.y - height) / height;
		//		position = new Vector3(pos.x, pos.y, 0.0f);

		//		// Position the cube.
		//		transform.position = position;
		//	}

		//	if (Input.touchCount == 2)
		//	{
		//		touch = Input.GetTouch(1);

		//		if (touch.phase == TouchPhase.Began)
		//		{
		//			// Halve the size of the cube.
		//			transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		//		}

		//		if (touch.phase == TouchPhase.Ended)
		//		{
		//			// Restore the regular size of the cube.
		//			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		//		}
		//	}
		//}
	}
}

