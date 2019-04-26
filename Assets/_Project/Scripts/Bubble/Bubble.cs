using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Bubble : MonoBehaviour, IPointerClickHandler, IDragHandler
{
	public static event Action<int> OnDestroy;

	public BubbleType Type { get; private set; }
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

	//----------------------------------------------------------------

	public void SetParentTubeID(int value)
	{
		_parentTubeID = value;
	}

	public void AddForce(float value)
	{
		_rigidbody2D.AddForce(Vector3.up * value, ForceMode2D.Impulse);
	}

	public void SetReleased()
	{
		IsReleased = true;
	}

	public void SelfDestroy()
	{
		GameController.Instance.sound.PlaySound(explosionSound);

		SpawnExplosion();

		OnDestroy?.Invoke(_parentTubeID);
		Destroy(gameObject);
	}

	private void SpawnExplosion()
	{
		Instantiate(_explosion, this.transform.position, Quaternion.identity);
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

	private void FixedUpdate()
	{
		//GetComponent<Rigidbody2D>().velocity = transform.up * (GameController.Instance.gameSettings.moveUpSpeed /** _baseSpeedTimer*/) * Time.deltaTime;
		transform.Translate(-transform.up * (_gameSettings.BubbleMoveSpeed /** _baseSpeedTimer*/) * 0.1f * Time.deltaTime);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (_selfDestroyStarted)
		{
			return;
		}

		GameController.Instance.sound.PlaySound(soundName01);

		_clickCount++;

		AddForce(_gameSettings.BounceRate);
		Enlarge();

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

	private void Init()
	{
		IsReleased = false;

		_rigidbody2D.drag = _gameSettings.DragRate;

		Type = (BubbleType)UnityEngine.Random.Range(0, 3);
		_state = BubbleState.Small;

		_renderer.material.color = _gameSettings.ColorDummy;
		SetColor(Type);

		if (_gameSettings.ColorMode == BubbleColorMode.Explicit)
		{
			_renderer.material.color = _color;
		}
	}

	private void SetColor(BubbleType bubbleType)
	{
		switch (bubbleType)
		{
			case BubbleType.A: _color = _gameSettings.ColorA; break;
			case BubbleType.B: _color = _gameSettings.ColorB; break;
			case BubbleType.C: _color = _gameSettings.ColorC; break;
		}
	}

	private void Enlarge()
	{
		if (_clickCount == _gameSettings.EnlargeSizeClickCount)
		{
			_view.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

			_state = BubbleState.Medium;
		}
		else if (_clickCount == _gameSettings.EnlargeSizeClickCount * 2)
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
		else if (_clickCount > _gameSettings.EnlargeSizeClickCount * 2)
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
		_dir.Normalize();
		_rigidbody2D.AddForce(_dir * _spoeedReflection, ForceMode2D.Impulse);
	}

	public class BubbleDIFactory : PlaceholderFactory<Bubble> { }
}