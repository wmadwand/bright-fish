using UnityEngine;

namespace BrightFish
{
	public class BubbleAlongPath : MonoBehaviour
	{
		public BubblePathFollower follower;

		public bool isBouncedUp;
		public float followerspeedBounced = -10;
		public float fadeRate = 5;
		public float targetSpeed;

		public float baseSpeed;

		public float bounceRateUp, bounceRateDown;

		float t;

		bool finishedBounce;
		bool isBouncedDown;

		bool isPlayerClick = false;

		private void Awake()
		{
			follower = GetComponent<BubblePathFollower>();
			baseSpeed = follower.speed;
		}

		private void Update()
		{
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

				t += bounceRateDown * Time.deltaTime;

				if (follower.speed >= baseSpeed)
				{
					finishedBounce = true;

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
			else if (finishedBounce && follower.speed > 0)
			{
				follower.speed = Mathf.Lerp(follower.speed, baseSpeed, t);
				t += bounceRateUp * Time.deltaTime;

				if (t > 1)
				{
					finishedBounce = false;
				}
			}
		}

		public void AddBounceForce(float value, bool isPlayerClick = true)
		{
			OnClick(value, isPlayerClick);
		}

		private void OnClick(float value, bool isPlayerClick = true)
		{
			this.isPlayerClick = isPlayerClick;

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