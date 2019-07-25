using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public class FishObstacleDetector : MonoBehaviour
	{
		public float distance = 3;
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

			if (hit1 && hit1.collider.GetComponent<FishPredator>() || hit2 && hit2.collider.GetComponent<FishPredator>())
			{
				_isPredatorFound = true;
			}
			else
			{
				_isPredatorFound = false;
			}
		}

		private RaycastHit2D CastRay(Vector3 direction)
		{
			return Physics2D.Raycast(transform.position, direction, distance, _fishLayer);
		}
	} 
}
