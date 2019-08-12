using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleAlongPath : MonoBehaviour, IPointerClickHandler
{
	PathFollower follower;

	public bool isBounced;
	float timer;
	public float timerTarget;
	public float followerspeedBounced = -10;
	public float fadeRate = 5;

	public float baseSpeed;

	float t;

	bool finishedBounce;

	private void Awake()
	{
		follower = GetComponent<PathFollower>();
		baseSpeed = follower.speed;
	}

	private void Update()
	{
		if (isBounced)
		{
			follower.speed = followerspeedBounced;
			follower.speed = Mathf.Lerp(follower.speed, 0, t);

			t += 0.5f * Time.deltaTime;

			if (follower.speed >= 0)
			{
				finishedBounce = true;

				isBounced = false;
				fadeRate = 5;
				t = 0;
			}
		}

		if (finishedBounce)
		{
			follower.speed = Mathf.Lerp(0, baseSpeed, t);
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

	void OnClick()
	{
		isBounced = true;
		finishedBounce = false;

		timer = timerTarget;
		t = 0;
		//follower.speed *= -1;

		Debug.Log("click");
	}
}
