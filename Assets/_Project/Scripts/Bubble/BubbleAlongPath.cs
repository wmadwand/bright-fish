using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrightFish
{
	public class BubbleAlongPath : MonoBehaviour, IPointerClickHandler
	{
		public BubblePathFollower follower;

		public bool isBouncedUp;
		//float timer;
		//public float timerTarget;
		public float followerspeedBounced = -10;
		public float fadeRate = 5;
		public float targetSpeed;

		public float baseSpeed = 4;

		float t;

		bool finishedBounce;
		bool isBouncedDown;

		private void Awake()
		{
			follower = GetComponent<BubblePathFollower>();
			baseSpeed = follower.speed;
		}

		private void Update()
		{
			if (isBouncedUp)
			{
				//follower.speed = followerspeedBounced;
				follower.speed = Mathf.Lerp(targetSpeed, 0, t);

				t += 0.5f * Time.deltaTime;

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

				t += 0.5f * Time.deltaTime;

				if (follower.speed >= baseSpeed)
				{
					finishedBounce = true;

					isBouncedDown = false;
					fadeRate = 5;
					t = 0;
				}
			}

			if (finishedBounce)
			{
				follower.speed = Mathf.Lerp(follower.speed, baseSpeed, t);
				t += 0.5f * Time.deltaTime;

				if (t > 1)
				{
					finishedBounce = false;
				}
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClick();
		}

		public void AddBounceForce(float value)
		{
			OnClick(value);
		}

		void OnClick(float value = 5)
		{
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
