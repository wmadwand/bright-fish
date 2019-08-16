using DG.Tweening;
using PathCreation;
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

		//TODO: move to separate class
		public int ScoreCount
		{
			get
			{
				int count = 0;

				switch (GetComponent<BubbleView>().State)
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
		public int _clickCount { get; private set; }
		private bool _selfDestroyStarted;

		private Rigidbody2D _rigidbody2D;
		private GameSettings _gameSettings;
		private GameObject _view;
		private float _selfDestroyTimeRate;

		private Food _childFood;

		private Level _currentLevelSettings;

		public event Action<bool> OnBounce;
		public event Action OnClickBubble;

		//----------------------------------------------------------------

		public void Init(Vector3 position, int id, Food food, PathCreator path)
		{
			transform.SetPositionAndRotation(position, Quaternion.identity);
			SetParentTubeID(id, food);

			GetComponent<BubbleAlongPath>().follower.pathCreator = path;
		}

		public void SetParentTubeID(int value, Food childFood)
		{
			_parentTubeID = value;
			_childFood = childFood;
		}

		public void AddBounceForce(float value, bool isPlayerClick = true)
		{
			//_rigidbody2D.AddForce(Vector3.up * value, ForceMode2D.Impulse);

			//GetComponent<BubbleAlongPath>().AddBounceForce(value, isPlayerClick);
			GetComponent<BubbleMovement>().AddForceDirection(value, isPlayerClick);
			OnBounce(isPlayerClick);
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
				//SpawnExplosion();
				GetComponent<BubbleView>().SpawnExplosion(_explosion, transform.position);
			}

			gameObject.SetActive(false);

			MessageBus.OnBubbleDestroy.Send(_parentTubeID);

			Destroy(gameObject);
		}

		//----------------------------------------------------------------

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Awake()
		{
			_rigidbody2D = GetComponentInChildren<Rigidbody2D>();

			_selfDestroyTimeRate = _gameSettings.SelfDestroyTime;

			_currentLevelSettings = GameController.Instance.levelController.CurrentLevel;

			PrivateInit();
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

			OnBounce(true);
			OnClickBubble();

			Debug.Log("click");
		}		

		private void PrivateInit()
		{
			IsReleased = false;

			_rigidbody2D.drag = _currentLevelSettings.DragRate;

			GetComponent<BubbleAlongPath>().follower.speed = _currentLevelSettings.BubbleMoveSpeed;
			GetComponent<BubbleAlongPath>().bounceRateDown = _currentLevelSettings.BounceRateDown;
			GetComponent<BubbleAlongPath>().bounceRateUp = _currentLevelSettings.BounceRateUp;

			var spawnPointsLength = GameController.Instance.fishSpawner.SpawnPoints.Length;

			Type = (ColorType)UnityEngine.Random.Range(0, spawnPointsLength);

			GetComponent<BubbleView>().Init(Type);
		}

		public void RevealFoodColor()
		{
			_childFood.RevealColor();
			Type = _childFood.Type;
			GetComponentInChildren<BoxCollider2D>().enabled = false;
		}

		//----------------------------------------------------------------

		public class BubbleDIFactory : PlaceholderFactory<Bubble> { }
	}
}