using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public class FishObstacleDetector : MonoBehaviour
	{
		[SerializeField] private float _distance = 3;
		private int _fishLayer;

		private RaycastHit2D _hit1;
		private RaycastHit2D _hit2;

		private bool _isPredatorFound;
		private FishView _fishView;

		//----------------------------------------------------------------

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
			_hit1 = CastRay(transform.right);
			_hit2 = CastRay(-transform.right);

			if (_hit1 && _hit1.collider.GetComponent<FishPredator>() && IsInObstacleSight(_hit1.transform)
				|| _hit2 && _hit2.collider.GetComponent<FishPredator>() && IsInObstacleSight(_hit2.transform))
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

		private bool IsInObstacleSight(Transform obstacleTr)
		{
			var dot = Vector3.Dot(this.transform.right.normalized, obstacleTr.right.normalized);

			return dot < 0f && transform.position.x < obstacleTr.position.x || dot > 0f && transform.position.x > obstacleTr.position.x;
		}

		private RaycastHit2D CastRay(Vector3 direction)
		{
			return Physics2D.Raycast(transform.position, direction, _distance, _fishLayer);
		}
	}
}