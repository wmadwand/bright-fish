using DG.Tweening;
using System;
using UnityEngine;

namespace BrightFish
{
	public class FishAttack : MonoBehaviour
	{
		public bool IsTargetDetected { get; private set; }
		public bool IsAttacking { get; private set; }

		[SerializeField] private float _attackMoveDistance = 2;

		public int damage = 10;
		public float distance = 3;
		public float _timeBetweenAttacks = 3f;

		private int _attackLayer;
		private float _nextAttackTime;

		private RaycastHit2D hit;

		private FishView _fishView;

		//----------------------------------------------------------------

		private void Awake()
		{
			Physics2D.queriesStartInColliders = false;

			_fishView = GetComponent<FishView>();

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

			//health.ChangeHealth(-damage);
			//Animate(health.ChangeHealth);

			_attackSequence = DOTween.Sequence()
			.Append(_fishView.View.transform.DOLocalMoveX(_attackMoveDistance, .2f).OnComplete(() => { Debug.Log("Punched!"); health.ChangeHealth(-damage); }))
			//.InsertCallback(.1f,() => { Debug.Log("Punched!"); health.ChangeHealth(-damage); })
			.Append(_fishView.View.transform.DOLocalMoveX(0, .2f))/*.SetAutoKill(false)*/;

		}

		Sequence _attackSequence;

		private void Animate(TweenCallback callback)
		{
			_attackSequence = DOTween.Sequence()
			.Append(_fishView.transform.DOLocalMoveX(5, .2f))
			.AppendCallback(() => callback())
			.Append(_fishView.transform.DOLocalMoveX(0, .2f));
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			var offset = transform.position + new Vector3(distance * transform.right.x, 0);
			Gizmos.DrawLine(transform.position, offset);
		}
	}
}