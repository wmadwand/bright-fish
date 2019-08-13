using DG.Tweening;
using System;
using System.Collections;
using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BrightFish
{
	public sealed class Bubble : MonoBehaviour
	{
		public ColorType Type { get; private set; }
		public bool IsReleased { get; private set; }

		//public int ParentTubeID => _parentTubeID;
		//TODO: move to separate class
		public int ScoreCount
		{
			get
			{
				int count = 0;

				switch (_state)
				{
					case BubbleState.Small:
						count = 50;
						break;
					case BubbleState.Medium:
						count = 100;
						break;
					case BubbleState.Big:
						count = 200;
						break;
					default:
						break;
				}

				return count;
			}
		}

		//TODO: move to separate class
		[SerializeField] private Sounds soundName01, explosionSound;

		[SerializeField] private GameObject _explosion;

		private int _parentTubeID;
		private BubbleState _state;
		private int _clickCount;
		private bool _selfDestroyStarted;

		private Color _color;
		private Renderer _renderer;
		private Rigidbody2D _rigidbody2D;
		private GameSettings _gameSettings;
		private GameObject _view;
		private float _selfDestroyTimeRate;

		private Food _childFood;

		private Level _currentLevelSettings;

		//----------------------------------------------------------------

		public void SetParentTubeID(int value, Food childFood)
		{
			_parentTubeID = value;
			_childFood = childFood;
		}

		public void AddBounceForce(float value, bool isPlayerClick = true)
		{
			//_rigidbody2D.AddForce(Vector3.up * value, ForceMode2D.Impulse);

			GetComponent<BubbleAlongPath>().AddBounceForce(value, isPlayerClick);
		}

		public void SetReleased()
		{
			IsReleased = true;
		}

		public void SelfDestroy(bool isReqiredExplosion = false, bool isRequiredBadSound = false)
		{
			if (isRequiredBadSound)
			{
				GameController.Instance.sound.PlaySound(explosionSound);
			}

			if (isReqiredExplosion)
			{
				SpawnExplosion();
			}

			gameObject.SetActive(false);

			MessageBus.OnBubbleDestroy.Send(_parentTubeID);

			Destroy(gameObject);
		}

		private void SpawnExplosion()
		{
			var go = Instantiate(_explosion, transform.position, Quaternion.identity);

			//Vector3 vec = new Vector3(transform.position.x, transform.position.y, -1);		

			//go.transform.SetPositionAndRotation(vec, Quaternion.identity);
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
			_rigidbody2D = GetComponentInChildren<Rigidbody2D>();
			_view = _renderer.gameObject;

			_selfDestroyTimeRate = _gameSettings.SelfDestroyTime;

			_currentLevelSettings = GameController.Instance.levelController.CurrentLevel;

			Init();
		}

		private void Update()
		{
			if (_selfDestroyStarted)
			{
				_selfDestroyTimeRate -= Time.fixedDeltaTime;

				if (_selfDestroyTimeRate <= 0)
				{
					SelfDestroy();
				}
			}
		}

		public void OnClick()
		{
			if (_selfDestroyStarted)
			{
				return;
			}

			if (_clickCount >= _currentLevelSettings.EnlargeSizeClickCount * 2 && !_gameSettings.DestroyBigBubbleClick)
			{
				return;
			}

			GameController.Instance.sound.PlaySound(soundName01);

			_clickCount++;

			//AddBounceForce(_currentLevelSettings.BounceRate);

			ShakeBubble();

			//Enlarge();
			Diffuse();

			Debug.Log("click");
		}

		public void ShakeBubble(bool isClick = true)
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

		private void Init()
		{
			IsReleased = false;

			_rigidbody2D.drag = _currentLevelSettings.DragRate;

			GetComponent<BubbleAlongPath>().follower.speed = _currentLevelSettings.BubbleMoveSpeed;
			GetComponent<BubbleAlongPath>().bounceRateDown = _currentLevelSettings.BounceRateDown;
			GetComponent<BubbleAlongPath>().bounceRateUp = _currentLevelSettings.BounceRateUp;

			var spawnPointsLength = GameController.Instance.fishSpawner.SpawnPoints.Length;

			Type = (ColorType)UnityEngine.Random.Range(0, spawnPointsLength);
			_state = BubbleState.Small;

			_renderer.material.color = _gameSettings.ColorDummy;
			_renderer.material.color = new Color(_renderer.material.color.a, _renderer.material.color.g, _renderer.material.color.b, .85f);

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

		private void Enlarge()
		{
			if (_clickCount == _currentLevelSettings.EnlargeSizeClickCount)
			{
				_view.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

				_state = BubbleState.Medium;
			}
			else if (_clickCount == _currentLevelSettings.EnlargeSizeClickCount * 2)
			{
				_view.transform.localScale = new Vector3(.7f, .7f, .7f);

				_state = BubbleState.Big;

				if (_gameSettings.BigBubbleSelfDestroy)
				{
					_selfDestroyStarted = true;
				}

				_renderer.material.color = _color;

				if (_selfDestroyStarted)
				{
					StartCoroutine(BlinkRoutine());
				}
			}
			else if (_gameSettings.DestroyBigBubbleClick && _clickCount > _currentLevelSettings.EnlargeSizeClickCount * 2)
			{
				SelfDestroy();
			}
		}

		private void Diffuse()
		{
			if (_clickCount == _currentLevelSettings.EnlargeSizeClickCount)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[0].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .7f);

				_state = BubbleState.Medium;
			}
			else if (_clickCount == _currentLevelSettings.EnlargeSizeClickCount * 2)
			{
				var size = _gameSettings.ClickEnlargeSizePairs[1].sizeRate;
				_view.transform.localScale = new Vector3(size, size, size);

				_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, .0f);

				_state = BubbleState.Big;


				_childFood.RevealColor();
				Type = _childFood.Type;
				GetComponentInChildren<BoxCollider2D>().enabled = false;


				if (_gameSettings.BigBubbleSelfDestroy)
				{
					_selfDestroyStarted = true;
				}

				if (_selfDestroyStarted)
				{
					StartCoroutine(BlinkRoutine());
				}
			}
			else if (_gameSettings.DestroyBigBubbleClick && _clickCount > _currentLevelSettings.EnlargeSizeClickCount * 2)
			{
				SelfDestroy();
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

		public void AddForceDirection(Vector2 _dir/*, float _speed*/)
		{
			GetComponent<BubbleAlongPath>().AddBounceForce(/*_dir.y * */_currentLevelSettings.SpeedReflection);

			//_dir.Normalize();
			//_rigidbody2D.AddForce(_dir * _currentLevelSettings.SpeedReflection, ForceMode2D.Impulse);
		}

		public void AddForceDirectionRB(Vector2 _dir/*, float _speed*/)
		{
			_dir.Normalize();
			_rigidbody2D.AddForce(_dir * _currentLevelSettings.SpeedReflection, ForceMode2D.Impulse);
		}

		//----------------------------------------------------------------

		public class BubbleDIFactory : PlaceholderFactory<Bubble> { }
	}
}