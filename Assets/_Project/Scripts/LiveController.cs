using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LiveController : MonoBehaviour
{
	public static event Action OnLivesOut;

	[SerializeField] private Image[] _heartImages;

	private int _livesCount;
	private GameSettings _gameSettings;


	public void ResetLives()
	{
		_livesCount = _gameSettings.LivesCount;
		Array.ForEach(_heartImages, item => item.color = Color.white);
	}

	[Inject]
	private void Construct(GameSettings gameSettings)
	{
		_gameSettings = gameSettings;
	}

	private void Awake()
	{
		_livesCount = _gameSettings.LivesCount;

		Fish.OnDeath += Fish_OnDeath;
		GameController.OnStart += GameController_OnStart;
	}

	private void GameController_OnStart()
	{
		ResetLives();
	}

	private void OnDestroy()
	{
		Fish.OnDeath -= Fish_OnDeath;
	}

	private void Fish_OnDeath(Fish fish, BubbleType arg1, Vector3 arg2)
	{
		if (_gameSettings.GodMode)
		{
			return;
		}

		_livesCount--;

		_heartImages[_livesCount].color = Color.black;

		if (_livesCount <= 0)
		{
			OnLivesOut?.Invoke();
		}
	}
}
