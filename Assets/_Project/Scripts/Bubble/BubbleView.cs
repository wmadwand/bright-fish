using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class BubbleView : MonoBehaviour
	{
		private GameObject _view;
		private Bubble _bubble;
		private Renderer _renderer;
		public BubbleState State { get; private set; }
		private GameSettings _gameSettings;
		private Color _color;

		private Level _currentLevelSettings;

		public bool IsBubbleBurst => State == BubbleState.Big;

		public void Init(ColorType type)
		{
			State = BubbleState.Small;

			_renderer = GetComponentInChildren<Renderer>();

			_renderer.material.color = _gameSettings.ColorDummy;
			_renderer.material.color = new Color(_renderer.material.color.a, _renderer.material.color.g, _renderer.material.color.b, .85f);

			SetColor(type);

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
		}

		private void OnDestroy()
		{
			_bubble.OnBounce -= Shake;
			_bubble.OnClickBubble -= Diffuse;
		}

		private void Shake(bool isClick = true)
		{
			if (isClick)
			{
				_view.transform.DOShakeScale(.4f, .2f, 10, 45);

			}
			else
			{
				_view.transform.DOShakeScale(.2f, .1f, 1, 5);
			}
		}

		private void Diffuse()
		{
			if (_bubble._clickCount == _currentLevelSettings.EnlargeSizeClickCount)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[0].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .7f);

				State = BubbleState.Medium;
			}
			else if (_bubble._clickCount == _currentLevelSettings.EnlargeSizeClickCount * 2)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[1].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .0f);

				State = BubbleState.Big;

				_bubble.RevealFoodColor();

				//if (_gameSettings.BigBubbleSelfDestroy)
				//{
				//	_selfDestroyStarted = true;
				//}

				//if (_selfDestroyStarted)
				//{
				//	StartCoroutine(BlinkRoutine());
				//}
			}
			else if (_gameSettings.DestroyBigBubbleClick && _bubble._clickCount > _currentLevelSettings.EnlargeSizeClickCount * 2)
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

		public void SpawnExplosion(GameObject template, Vector2 postion)
		{
			var go = Instantiate(template, postion, Quaternion.identity);
		}
	}
}