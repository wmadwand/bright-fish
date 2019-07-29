using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BrightFish
{
	public class FishHealth : MonoBehaviour
	{
		public int Value { get; private set; } = 50;

		float timer;
		public float countdownRate = 2;

		private Fish _fish;

		private void Awake()
		{
			_fish = GetComponent<Fish>();
		}

		public void OnGetDamage()
		{
			_fish.UpdateHealthBar(Value);

			if (IsDead)
			{
				_fish.Destroy();
			}
		}

		public bool IsFedUp
		{
			get
			{
				return Value >= 100;
			}
		}

		public bool IsDead
		{
			get
			{
				return Value <= 0;
			}
		}

		private void Update()
		{
			if (Value == 100)
			{
				return;
			}

			if (Time.time > timer)
			{
				if (Value > 0)
				{
					Value--;
					timer = Time.time + countdownRate;
					_fish.UpdateHealthBar(Value);
				}
				else
				{

				}
			}
		}

		public void ChangeHealth(int value)
		{
			this.Value += value;
		}
	} 
}
