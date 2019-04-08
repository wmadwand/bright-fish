using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Vector3 position;
	private float width;
	private float height;

	bool dragStarted;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if ((eventData != null && eventData.used))
		{
			return;
		}

		dragStarted = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		////Very nice for 2D objects dragging
		//transform.position = eventData.position;


		// Solution #01
		Plane plane = new Plane(Vector3.forward, transform.position);
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

		if (plane.Raycast(ray, out float distamce))
		{
			transform.position = ray.origin + ray.direction * distamce;
		}

		// Solution #02
		//Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
		//Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
		//Vector3 PN = -Camera.main.transform.forward; // Take current negative camera's forward as Plane's Normal
		//float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
		//Vector3 P = R.origin + R.direction * t; // Find the new point.

		//transform.position = P;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		dragStarted = false;
	}

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

		//	////if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		//	////{

		//	Ray ray = Camera.main.ScreenPointToRay(touch.position);

		//	if (Physics.Raycast(ray, out RaycastHit hit, 1 << LayerMask.NameToLayer("Draggable")))
		//	{
		//		if (touch.phase == TouchPhase.Moved)
		//		{
		//			var vec = new Vector3(touch.position.x, touch.position.y, 0);

		//			hit.transform.position = Camera.main.WorldToScreenPoint(vec);
		//			Debug.Log($"vec: {vec}");
		//		}
		//	}

		//	// Move the cube if the screen has the finger moving.
		//	//if (touch.phase == TouchPhase.Moved)
		//	//{
		//	//	Vector2 pos = touch.position;
		//	//	//pos.x = (pos.x - width) / width;
		//	//	//pos.y = (pos.y - height) / height;
		//	//	position = new Vector3(pos.x, pos.y, 0.0f);

		//	//	// Position the cube.
		//	//	transform.position = position;
		//	//}

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

