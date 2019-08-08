using System;
using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BrightFish
{
	public class LiveController : MonoBehaviour
	{
		[SerializeField] private Image[] _heartImages;

		private int _livesCount;
		private GameSettings _gameSettings;

		//----------------------------------------------------------------

		public void ResetLives()
		{
			_livesCount = _gameSettings.LivesCount;
			Array.ForEach(_heartImages, item => item.color = Color.white);
		}

		//----------------------------------------------------------------

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Awake()
		{
			_livesCount = _gameSettings.LivesCount;

			MessageBus.OnFishDying.Receive += Fish_OnDeath;
			MessageBus.OnGameStart.Receive += GameController_OnStart;
		}

		private void OnDestroy()
		{
			MessageBus.OnFishDying.Receive -= Fish_OnDeath;
			MessageBus.OnGameStart.Receive -= GameController_OnStart;
		}

		private void GameController_OnStart()
		{
			ResetLives();
		}

		private void Fish_OnDeath(/*Fish fish, BubbleType arg1, Vector3 arg2*/)
		{
			if (_gameSettings.GodMode)
			{
				return;
			}

			_livesCount--;

			_heartImages[_livesCount].color = Color.black;

			if (_livesCount <= 0)
			{
				MessageBus.OnPlayerLivesOut.Send();
			}
		}
	}
}