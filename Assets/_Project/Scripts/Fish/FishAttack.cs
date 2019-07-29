﻿using DG.Tweening;
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
		private Fish _fish;

		private FishHealth _targetFishHealth;
		private Fish _targetFish;

		private Sequence _attackSequence;

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
				StartAttack();

				_nextAttackTime = Time.time + _timeBetweenAttacks;
			}
		}

		private RaycastHit2D CastRay()
		{
			return Physics2D.Raycast(transform.position, transform.right, distance, _attackLayer);
		}

		private void StartAttack()
		{
			hit = CastRay();

			if (!hit || hit && hit.collider.GetComponent<FishHealth>() == null)
			{
				IsAttacking = false;
				return;
			}

			IsAttacking = true;

			_targetFishHealth = hit.collider.GetComponent<FishHealth>();
			_targetFish = hit.collider.GetComponent<Fish>();

			AnimateBite(MakeDamage);
		}

		private void MakeDamage()
		{
			Debug.Log("Punched!");
			_targetFishHealth.ChangeHealth(-damage);
			_targetFish.UpdateHealthBar(_targetFishHealth.Value);
		}

		private void AnimateBite(TweenCallback callback)
		{
			_attackSequence = DOTween.Sequence()
			.Append(_fishView.View.transform.DOLocalMoveX(_attackMoveDistance, .2f))
			.AppendCallback(() => callback())
			.Append(_fishView.View.transform.DOLocalMoveX(0, .2f));

			//_attackSequence = DOTween.Sequence()
			//.Append(_fishView.View.transform.DOLocalMoveX(_attackMoveDistance, .2f).OnComplete(() => { TwCallback(); }))
			////.InsertCallback(.1f,() => { Debug.Log("Punched!"); health.ChangeHealth(-damage); })
			//.Append(_fishView.View.transform.DOLocalMoveX(0, .2f))/*.SetAutoKill(false)*/;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			var offset = transform.position + new Vector3(distance * transform.right.x, 0);
			Gizmos.DrawLine(transform.position, offset);
		}
	}
}