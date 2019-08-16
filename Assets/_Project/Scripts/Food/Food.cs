using System;
using System.Collections;
using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Terminus.Extensions;

namespace BrightFish
{
	public class Food : MonoBehaviour, IPointerClickHandler, IDragHandler
	{
		//public static event Action<int> OnDestroy;

		public ColorType Type { get; private set; }
		public bool IsReleased { get; private set; }

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

		[SerializeField] private float _spoeedReflection;

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

		private Level _currentLevelSettings;

		//----------------------------------------------------------------

		public void SetParentTubeID(int value)
		{
			_parentTubeID = value;
		}

		public void SetParentRigidBody(Rigidbody2D rigidbody2D)
		{
			//_rigidbody2D = rigidbody2D;
		}

		public void AddForce(float value)
		{
			_rigidbody2D.AddForce(Vector3.up * value, ForceMode2D.Impulse);
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

			//OnDestroy?.Invoke(_parentTubeID);

			MessageBus.OnFoodDestroy.Send(_parentTubeID);

			Destroy(gameObject);
		}

		private void SpawnExplosion()
		{
			var go = Instantiate(_explosion, transform.position, Quaternion.identity);

			//Vector3 vec = new Vector3(transform.position.x, transform.position.y, -1);		

			//go.transform.SetPositionAndRotation(vec, Quaternion.identity);
		}

		public void AddForceDirection(Vector2 _dir/*, float _speed*/)
		{
			_dir.Normalize();
			_rigidbody2D.AddForce(_dir * _spoeedReflection, ForceMode2D.Impulse);
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
			_selfDestroyTimeRate = _gameSettings.SelfDestroyTime;

			_currentLevelSettings = GameController.Instance.levelController.CurrentLevel;

			SetCollidersActive(false);
		}

		public void SetCollidersActive(bool value)
		{
			foreach (var item in GetComponentsInChildren<Collider2D>())
			{
				item.enabled = value;
			}
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

		private void FixedUpdate()
		{
			//GetComponent<Rigidbody2D>().velocity = transform.up * (GameController.Instance.gameSettings.moveUpSpeed /** _baseSpeedTimer*/) * Time.deltaTime;
			//transform.Translate(-transform.up * (_gameSettings.BubbleMoveSpeed /** _baseSpeedTimer*/) * 0.1f * Time.deltaTime);
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			GameController.Instance.sound.PlaySound(soundName01);

			return;


			if (_selfDestroyStarted)
			{
				return;
			}

			if (_clickCount >= _currentLevelSettings.BubbleEnlargeSizeClickCount * 2 && !_gameSettings.DestroyBigBubbleClick)
			{
				return;
			}

			GameController.Instance.sound.PlaySound(soundName01);

			_clickCount++;

			
			//Enlarge();

			Debug.Log("click");
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			//return;

			////Very nice approach for UI objects dragging
			//transform.position = eventData.position;


			// Solution #01 !!!! WORKING ONE
			//Plane plane = new Plane(Vector3.forward, transform.position);
			//Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

			//if (plane.Raycast(ray, out float distance))
			//{
			//	transform.position = ray.origin + ray.direction * distance;
			//}

			// Solution #02
			//Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
			//Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
			//Vector3 PN = -Camera.main.transform.forward; // Take current negative camera's forward as Plane's Normal
			//float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
			//Vector3 P = R.origin + R.direction * t; // Find the new point.
			//transform.position = P;
		}

		public void Init(Rigidbody2D rigidbody2D)
		{
			_rigidbody2D = rigidbody2D;

			IsReleased = false;

			_rigidbody2D.drag = _currentLevelSettings.DragRate;

			var spawnPointsLength = GameController.Instance.fishSpawner.SpawnPoints.Length;

			Type = GameController.Instance.levelController.CurrentLevel.ColorTypes.GetRandom();

			_state = BubbleState.Small;

			_renderer.material.color = _gameSettings.ColorDummy;
			SetColor(Type);

			if (_gameSettings.ColorMode == BubbleColorMode.Explicit)
			{
				_renderer.material.color = _color;
			}
		}

		public void RevealColor()
		{
			_renderer.material.color = _color;
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

		//----------------------------------------------------------------

		public class FoodDIFactory : PlaceholderFactory<Food> { }
	}
}