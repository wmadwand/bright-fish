using UnityEngine;

namespace BrightFish
{
	public class FishAttack : MonoBehaviour
	{
		public bool IsTargetDetected { get; private set; }
		public bool IsAttacking { get; private set; }

		public int damage = 10;
		public float distance = 3;
		public float _timeBetweenAttacks = 3f;

		private int _attackLayer;
		private float _nextAttackTime;

		private RaycastHit2D hit;

		//----------------------------------------------------------------

		private void Awake()
		{
			Physics2D.queriesStartInColliders = false;

			_attackLayer = 1 << LayerMask.NameToLayer("Fish");
		}

		private void Update()
		{
			hit = CastRay();
			IsTargetDetected = hit && hit.collider.GetComponent<FishHealth>() ? true : false;

			if (IsTargetDetected && Time.time > _nextAttackTime)
			{
				MakeDamage();

				_nextAttackTime = Time.time + _timeBetweenAttacks;
			}
		}

		private RaycastHit2D CastRay()
		{
			return Physics2D.Raycast(transform.position, transform.right, distance, _attackLayer);
		}

		private void MakeDamage()
		{
			hit = CastRay();

			if (!hit || hit && hit.collider.GetComponent<FishHealth>() == null)
			{
				IsAttacking = false;
				return;
			}

			IsAttacking = true;

			var health = hit.collider.GetComponent<FishHealth>();

			health.ChangeHealth(-damage);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			var offset = transform.position + new Vector3(distance * transform.right.x, 0);
			Gizmos.DrawLine(transform.position, offset);
		}
	}
}