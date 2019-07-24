using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BrightFish
{
	public class FishHealth : MonoBehaviour
	{
		public int value = 50;

		float timer;
		public float countdownRate = 2;

		private Fish _fish;

		private void Awake()
		{
			_fish = GetComponent<Fish>();
		}

		public void OnGetDamage()
		{
			_fish.UpdateHealthBar(value);

			if (IsDead)
			{
				_fish.Destroy();
			}
		}

		public bool IsFedUp
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
					_fish.UpdateHealthBar(value);
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
}
