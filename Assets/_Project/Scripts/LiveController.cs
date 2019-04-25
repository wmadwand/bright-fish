using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LiveController : MonoBehaviour
{
	public static Action OnLivesOut;

	[SerializeField] Image[] _heartImages;

	private int _livesCount;
	private GameSettings _gameSettings;

	[Inject]
	private void Construct(GameSettings gameSettings)
	{
		_gameSettings = gameSettings;
	}

	private void Awake()
	{
		_livesCount = _gameSettings.LivesCount;

		Fish.OnDeath += Fish_OnDeath;
	}
	private void OnDestroy()
	{
		Fish.OnDeath -= Fish_OnDeath;
	}

	private void Fish_OnDeath(BubbleType arg1, Vector3 arg2)
	{
		_livesCount--;

		if (_livesCount <= 0)
		{
			OnLivesOut?.Invoke();
		}
	}
}
