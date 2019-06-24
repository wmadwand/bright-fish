using System;
using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelController : MonoBehaviour
{
	//public static event Action OnLevelComplete;
	//public static event Action OnLevelFail;

	[SerializeField] private Text _rescuedFishText;

	private int _rescuedFishTargetCount;
	private int _rescuedFishCurrentCount;

	private GameSettings _gameSettings;


	public void ResetLevel()
	{
		_rescuedFishTargetCount = _gameSettings.RescuedFishTargetCount;
		_rescuedFishCurrentCount = 0;

		UpdateText();
	}

	[Inject]
	private void Construct(GameSettings gameSettings)
	{
		_gameSettings = gameSettings;
	}

	private void Awake()
	{
		ResetLevel();

		//Fish.OnHappy += Fish_OnHappy;
		//LiveController.OnLivesOut += LiveController_OnLivesOut;
		//GameController.OnStart += GameController_OnStart;

		MessageBus.OnFishRescued.Receive += Fish_OnHappy;
		MessageBus.OnPlayerLivesOut.Receive += LiveController_OnLivesOut;
		MessageBus.OnGameStart.Receive += GameController_OnStart;
	}

	private void GameController_OnStart()
	{
		ResetLevel();
	}

	private void LiveController_OnLivesOut()
	{
		//OnLevelFail?.Invoke();
		MessageBus.OnLevelFailed.Send();
	}

	private void Fish_OnHappy(Fish fish,BubbleType arg1, Vector3 arg2)
	{
		_rescuedFishCurrentCount++;
		UpdateText();

		if (_rescuedFishCurrentCount >= _rescuedFishTargetCount)
		{
			//OnLevelComplete?.Invoke();
			MessageBus.OnLevelComplete.Send();
		}
	}

	private void OnDestroy()
	{
		//Fish.OnHappy -= Fish_OnHappy;
		MessageBus.OnFishRescued.Receive += Fish_OnHappy;
		MessageBus.OnPlayerLivesOut.Receive -= LiveController_OnLivesOut;
		MessageBus.OnGameStart.Receive -= GameController_OnStart;
	}

	void UpdateText()
	{
		_rescuedFishText.text = $"{_rescuedFishCurrentCount}/{_rescuedFishTargetCount}";
	}
}