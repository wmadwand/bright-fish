using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class BubbleMovement : MonoBehaviour
	{
		public BubblePathFollower follower;

		private bool _isBouncedUp;
		private bool _isBouncedDown;
		private bool _isBounceFinished;

		private float _targetSpeed;
		private float _baseSpeed;
		private float _velocity = 0.2f;

		private float _bounceRateUp, _bounceRateDown;
		private float _t;

		private bool _isClickGesture = false;
		private bool _isSwipeGesture;

		private Level _currentLevelSettings;
		private TubeSettings _tubeSettings;

		private float _speedPrev;
		bool isPaused;

		//----------------------------------------------------------------		

		public void Init(TubeSettings tubeSettings)
		{
			_tubeSettings = tubeSettings;
			follower.speed = _tubeSettings.bubbleBaseSpeed /** -1*/;
			_baseSpeed = follower.speed;
		}

		private void Awake()
		{
			_currentLevelSettings = GameController.Instance.levelController.CurrentLevel;

			_bounceRateDown = _currentLevelSettings.BubbleBounceDownRate * -1;
			_bounceRateUp = _currentLevelSettings.BubbleBounceUpRate;

			follower = GetComponent<BubblePathFollower>();

			GetComponent<BubbleInteraction>().OnInteract += AddBounceForce;
			MessageBus.OnGamePause.Receive += OnGamePause_Receive;
		}

		private void OnGamePause_Receive(bool value)
		{
			if (value)
			{
				_speedPrev = follower.speed;
				follower.speed = 0;

				isPaused = true;
			}
			else
			{
				follower.speed = _speedPrev;

				isPaused = false;
			}
		}

		private void OnDestroy()
		{
			GetComponent<BubbleInteraction>().OnInteract -= AddBounceForce;

			MessageBus.OnGamePause.Receive -= OnGamePause_Receive;
		}

		private void Update()
		{
			if (isPaused)
			{
				return;
			}

			if (_isSwipeGesture)
			{
				follower.speed = _targetSpeed;
				return;
			}

			if (_isBouncedUp)
			{
				follower.speed = Mathf.Lerp(_targetSpeed * -1, 0, _t);

				_t += _bounceRateUp * Time.fixedDeltaTime;

				if (follower.speed >= 0)
				{
					_isBounceFinished = true;
					_isBouncedUp = false;
					_t = 0;
				}
			}
			else if (_isBouncedDown)
			{
				follower.speed = Mathf.Lerp(_targetSpeed * -1, _baseSpeed, _t);

				_t += _velocity + (_bounceRateDown * Time.fixedDeltaTime);

				if (follower.speed <= _baseSpeed)
				{
					_isBounceFinished = true;
					follower.speed = _baseSpeed;
					_isBouncedDown = false;
					_t = 0;
				}
			}

			if (_isClickGesture && _isBounceFinished && follower.speed >= 0)
			{
				follower.speed = Mathf.Lerp(0, _baseSpeed, _t);
				_t += _bounceRateUp * Time.fixedDeltaTime;

				if (_t > 1)
				{
					_isBounceFinished = false;
					_isClickGesture = false;
				}
			}
			else if (!_isSwipeGesture && _isBounceFinished && follower.speed > 0)
			{
				follower.speed = Mathf.Lerp(follower.speed, _baseSpeed, _t);
				_t += _bounceRateUp * Time.fixedDeltaTime;

				if (_t > 1)
				{
					_isBounceFinished = false;
				}
			}
		}

		public void AddBounceForce(float value = 1, bool isPlayerClick = true, bool isSwipe = false)
		{
			this._isClickGesture = isPlayerClick;
			_isSwipeGesture = isSwipe;

			if (value < 0)
			{
				_isBouncedDown = true;
				_isBouncedUp = false;
			}
			else
			{
				_isBouncedDown = false;
				_isBouncedUp = true;
			}

			_isBounceFinished = false;
			_targetSpeed = value;

			_t = 0;

			Debug.Log("click");
		}
	}
}