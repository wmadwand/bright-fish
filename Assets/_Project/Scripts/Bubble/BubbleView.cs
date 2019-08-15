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
		public BubbleState _state { get; private set; }
		private GameSettings _gameSettings;

		private Level _currentLevelSettings;

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

				_state = BubbleState.Medium;
			}
			else if (_bubble._clickCount == _currentLevelSettings.EnlargeSizeClickCount * 2)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[1].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .0f);

				_state = BubbleState.Big;


				//_childFood.RevealColor();
				//Type = _childFood.Type;
				//GetComponentInChildren<BoxCollider2D>().enabled = false;

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

	}
}