using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
	public int value = 50;

	float timer;
	public float countdownRate = 2;

	//async void ChangeHEalth()
	//{
	//	while (value > 0)
	//	{
	//		await Task.Delay(TimeSpan.FromSeconds(delayRate));
	//	}


	//}

	private Fish _enemy;

	private void Awake()
	{
		_enemy = GetComponent<Fish>();
	}

	public void OnGetDamage()
	{
		_enemy.UpdateHealthBar(value);

		if (IsDead)
		{
			_enemy.Destroy();
		}
	}

	public bool IsFedup
	{
		get
		{
			return value >= 100;
		}
	}

	public bool IsDead
	{
		get
		{
			return value <= 0;
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
				_enemy.UpdateHealthBar(value);
			}
			else
			{

			}
		}


	}

	public void ChangeHealth(int value)
	{
		this.value += value;
	}
}
