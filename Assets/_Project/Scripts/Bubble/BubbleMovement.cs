using UnityEngine;

namespace BrightFish
{
	public class BubbleMovement : MonoBehaviour
	{
		public BubblePathFollower follower;

		public bool isBouncedUp;		
		public float fadeRate = 5;
		public float targetSpeed;

		public float baseSpeed;

		public float bounceRateUp, bounceRateDown;

		private float t;

		private bool finishedBounce;
		private bool isBouncedDown;

		private bool isPlayerClick = false;

		public float velocity = 0.2f;

		private bool _isSwipe;

		private void Awake()
		{
			follower = GetComponent<BubblePathFollower>();
			baseSpeed = follower.speed;
		}

		private void Update()
		{
			if (_isSwipe)
			{
				follower.speed = targetSpeed;
				return;
			}

			if (isBouncedUp)
			{
				follower.speed = Mathf.Lerp(targetSpeed, 0, t);

				t += bounceRateUp * Time.deltaTime;

				if (follower.speed <= 0)
				{
					finishedBounce = true;

					isBouncedUp = false;
					fadeRate = 5;
					t = 0;
				}
			}
			else if (isBouncedDown)
			{
				follower.speed = Mathf.Lerp(targetSpeed, baseSpeed, t);

				t += velocity + (bounceRateDown * Time.fixedDeltaTime);

				if (follower.speed >= baseSpeed)
				{
					finishedBounce = true;

					follower.speed = baseSpeed;

					isBouncedDown = false;
					fadeRate = 5;
					t = 0;
				}
			}

			if (isPlayerClick && finishedBounce && follower.speed <= 0)
			{
				follower.speed = Mathf.Lerp(0, baseSpeed, t);
				t += bounceRateUp * Time.deltaTime;

				if (t > 1)
				{
					finishedBounce = false;
					isPlayerClick = false;
				}
			}
			else if (!_isSwipe && finishedBounce && follower.speed > 0)
			{
				follower.speed = Mathf.Lerp(follower.speed, baseSpeed, t);
				t += bounceRateUp * Time.deltaTime;

				if (t > 1)
				{
					finishedBounce = false;
				}
			}
		}

		public void AddBounceForce(float value, bool isPlayerClick = true, bool isSwipe = false)
		{
			this.isPlayerClick = isPlayerClick;			
			_isSwipe = isSwipe;

			if (value < 0)
			{
				isBouncedDown = true;
				isBouncedUp = false;
			}
			else
			{
				isBouncedDown = false;
				isBouncedUp = true;
			}

			finishedBounce = false;
			targetSpeed = value;

			t = 0;

			Debug.Log("click");
		}		
	}
}