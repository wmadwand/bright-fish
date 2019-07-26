using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public class FishObstacleDetector : MonoBehaviour
	{
		[SerializeField] private float _distance = 3;
		private int _fishLayer;

		private RaycastHit2D hit1;
		private RaycastHit2D hit2;

		private bool _isPredatorFound;
		private FishView _fishView;

		private void Awake()
		{
			_fishView = GetComponent<FishView>();

			Physics2D.queriesStartInColliders = false;

			_fishLayer = 1 << LayerMask.NameToLayer("Fish");
		}

		private void Update()
		{
			CheckArea();
		}

		private void CheckArea()
		{
			hit1 = CastRay(transform.right);
			hit2 = CastRay(-transform.right);

			if (hit1 && hit1.collider.GetComponent<FishPredator>() && IsFaceToFaceWithObject(hit1.transform)
				|| hit2 && hit2.collider.GetComponent<FishPredator>() && IsFaceToFaceWithObject(hit2.transform))
			{
				_isPredatorFound = true;
				_fishView.OnPredatorFound();

				Debug.Log("OnPredatorFound");
			}
			else
			{
				_isPredatorFound = false;
				_fishView.OnPredatorLost();

				Debug.Log("OnPredatorLost");
			}
		}

		private bool IsFaceToFaceWithObject(Transform tr)
		{
			var isObjectFaceForward = Vector3.Dot(transform.right.normalized, tr.right.normalized);

			return isObjectFaceForward < 0f && transform.position.x < tr.position.x || isObjectFaceForward > 0f && transform.position.x > tr.position.x;
		}

		private RaycastHit2D CastRay(Vector3 direction)
		{
			return Physics2D.Raycast(transform.position, direction, _distance, _fishLayer);
		}
	}
}