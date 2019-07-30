using UnityEngine;

namespace BrightFish
{
	[RequireComponent(typeof(FishAttack))]
	[RequireComponent(typeof(FishRotator))]

	public class FishPredator : MonoBehaviour
	{
		public bool VampireVictimHealthOnAttack => _vampireVictimHealthOnAttack;

		[SerializeField] private float _timeBeforeRotate = 5f;
		[SerializeField] private bool _vampireVictimHealthOnAttack = false;

		private FishAttack _fishAttack;
		private FishRotator _fishRotator;		
		private float _time;

		private void Awake()
		{
			_fishAttack = GetComponent<FishAttack>();
			_fishRotator = GetComponent<FishRotator>();
		}

		private void Update()
		{
			if (_fishAttack.IsTargetDetected)
			{
				_time = 0;
				return;
			}

			_time += Time.deltaTime;

			if (_time >= _timeBeforeRotate)
			{
				_time = 0;
				_fishRotator.Rotate();
			}
		}
	}
}