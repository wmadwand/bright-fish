﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class BubbleView : MonoBehaviour
	{
		public ColorType Type { get; private set; }
		public bool IsBubbleBurst => State == BubbleState.Big;

		private GameObject _view;
		private Bubble _bubble;
		private Renderer _renderer;
		public BubbleState State { get; private set; }
		private GameSettings _gameSettings;
		private Color _color;
		private Level _currentLevelSettings;
		public Food _childFood;

		//----------------------------------------------------------------

		public bool IsProperColorType(ColorType type)
		{
			return Type == type;
		}

		public void SpawnExplosion(GameObject template, Vector2 postion)
		{
			Instantiate(template, postion, Quaternion.identity);
		}

		//----------------------------------------------------------------

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Awake()
		{
			_renderer = GetComponentInChildren<Renderer>();
			_view = _renderer.gameObject;
			_bubble = GetComponent<Bubble>();
			_currentLevelSettings = GameController.Instance.levelController.CurrentLevel;

			_bubble.OnBounce += Shake;
			_bubble.OnClickBubble += Diffuse;

			Init();
		}

		private void OnDestroy()
		{
			_bubble.OnBounce -= Shake;
			_bubble.OnClickBubble -= Diffuse;
		}

		private void Init()
		{
			State = BubbleState.Small;

			_renderer.material.color = _gameSettings.ColorDummy;
			_renderer.material.color = new Color(_renderer.material.color.a, _renderer.material.color.g, _renderer.material.color.b, .85f);

			var spawnPointsLength = GameController.Instance.fishSpawner.SpawnPoints.Length;
			Type = (ColorType)UnityEngine.Random.Range(0, spawnPointsLength);
			SetColor(Type);

			if (_gameSettings.ColorMode == BubbleColorMode.Explicit)
			{
				_renderer.material.color = _color;
			}
		}

		private void SetColor(ColorType bubbleType)
		{
			switch (bubbleType)
			{
				case ColorType.A: _color = _gameSettings.ColorA; break;
				case ColorType.B: _color = _gameSettings.ColorB; break;
				case ColorType.C: _color = _gameSettings.ColorC; break;
				case ColorType.D: _color = _gameSettings.ColorD; break;
				case ColorType.E: _color = _gameSettings.ColorE; break;
			}
		}

		private void Shake(bool isClick = true, TweenCallback callback = null)
		{
			if (isClick)
			{
				_view.transform.DOShakeScale(.4f, .2f, 10, 45).OnComplete(callback);

			}
			else
			{
				_view.transform.DOShakeScale(.2f, .1f, 1, 5);
			}
		}

		private void Diffuse()
		{
			if (_bubble._clickCount == _currentLevelSettings.BubbleEnlargeSizeClickCount)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[0].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .7f);

				Shake(true, () =>
				{
					State = BubbleState.Medium;
				});

				//_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .7f);
				//State = BubbleState.Medium;
			}
			else if (_bubble._clickCount == _currentLevelSettings.BubbleEnlargeSizeClickCount * 2)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[1].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .4f);

				Shake(true, () =>
				{
					_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .0f);
					State = BubbleState.Big;
					RevealFoodColor();
				});

				//_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .0f);
				//State = BubbleState.Big;
				//RevealFoodColor();



				//if (_gameSettings.BigBubbleSelfDestroy)
				//{
				//	_selfDestroyStarted = true;
				//}

				//if (_selfDestroyStarted)
				//{
				//	StartCoroutine(BlinkRoutine());
				//}
			}
			else if (_gameSettings.DestroyBigBubbleClick && _bubble._clickCount > _currentLevelSettings.BubbleEnlargeSizeClickCount * 2)
			{
				_bubble.SelfDestroy();
			}
		}

		private IEnumerator BlinkRoutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(_gameSettings.BlinkRate);

				_renderer.material.color = new Color(_color.r, _color.g, _color.b, 0);

				yield return new WaitForSeconds(_gameSettings.BlinkRate);

				_renderer.material.color = new Color(_color.r, _color.g, _color.b, 100);
			}
		}

		private void RevealFoodColor()
		{
			_childFood.RevealColor();
			Type = _childFood.Type;
			_childFood.GetComponentInChildren<BoxCollider2D>().enabled = false;
		}
	}
}