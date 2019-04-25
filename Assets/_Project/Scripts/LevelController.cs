using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelController : MonoBehaviour
{
	public static Action OnLevelComplete;
	public static Action OnLevelFail;

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

		Fish.OnHappy += Fish_OnHappy;
	}

	private void Fish_OnHappy(Fish fish,BubbleType arg1, Vector3 arg2)
	{
		_rescuedFishCurrentCount++;
		UpdateText();

		if (_rescuedFishCurrentCount >= _rescuedFishTargetCount)
		{
			OnLevelComplete?.Invoke();
		}
	}

	private void OnDestroy()
	{
		Fish.OnHappy -= Fish_OnHappy;
	}

	void UpdateText()
	{
		_rescuedFishText.text = $"{_rescuedFishCurrentCount}/{_rescuedFishTargetCount}";
	}
}