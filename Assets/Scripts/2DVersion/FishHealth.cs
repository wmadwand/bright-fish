using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
	private float value = 50;

	float timer;
	public float countdownRate = 2;

	//async void ChangeHEalth()
	//{
	//	while (value > 0)
	//	{
	//		await Task.Delay(TimeSpan.FromSeconds(delayRate));
	//	}


	//}

	public bool IsFedup
	{
		get
		{
			return value >= 100;
		}
	}

	private void Update()
	{
		if (value == 100)
		{
			return;
		}

		if (Time.time > timer)
		{
			if (value > 0)
			{
				value--;
				timer = Time.time + countdownRate;
			}
			else
			{

			}
		}


	}

	public void ChangeHealth(float value)
	{
		this.value += value;
	}
}
