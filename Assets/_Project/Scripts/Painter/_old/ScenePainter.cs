using System;
using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class ScenePainter : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer[] _spriteRenderers;

		private GameSettings _gameSettings;
		private float _saturationIntence = 1;

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Start()
		{
			MessageBus.OnFishRescued.Receive += OnFishRescued_Receive;

			foreach (var item in _spriteRenderers)
			{
				item.material.SetFloat("_Intensity", _saturationIntence);
			}
		}

		private void OnFishRescued_Receive(Fish arg1, ColorType arg2, Vector3 arg3)
		{
			//_saturationIntence -= _gameSettings.RescuedFishTargetCount * .01f;

			foreach (var item in _spriteRenderers)
			{
				item.material.SetFloat("_Intensity", _saturationIntence);
			}
		}

		private void OnDestroy()
		{
			MessageBus.OnFishRescued.Receive -= OnFishRescued_Receive;
		}
	}
}