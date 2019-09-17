using BrightFish;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCollision : MonoBehaviour
{
	[SerializeField] private float _distance = 3;

	private RaycastHit2D _hit1;
	private RaycastHit2D _hit2;

	private void Awake()
	{
		Physics2D.queriesStartInColliders = false;
		Physics2D.alwaysShowColliders = true;
		Physics2D.contactArrowScale = 3;		
	}

	private void Update()
	{
		CheckForContact2();
	}

	private void CheckForContact2()
	{
		_hit1 = CastRay(transform.right);
		_hit2 = CastRay(-transform.right);

		Debug.DrawLine(transform.position, new Vector3(transform.position.x + _distance, transform.position.y));
		Debug.DrawLine(transform.position, new Vector3(transform.position.x - _distance, transform.position.y));

		if (_hit1 /*&& _hit1.collider.GetComponent<Fish>()*/ || _hit2 /*&& _hit2.collider.GetComponent<Fish>()*/)
		{
			Debug.Log("OnContact");
		}

	}

	private RaycastHit2D CastRay(Vector3 direction)
	{
		return Physics2D.Raycast(transform.position, direction, _distance);
	}
}

