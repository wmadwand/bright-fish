using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Bubble : MonoBehaviour, IPointerClickHandler, IDragHandler
{
	public static event Action<int> OnDestroy;

	public bool IsReleased { get; private set; }

	public int ScoreCount
	{
		get
		{
			int count = 0;

			switch (_state)
			{
				case CoinState.Small:
					count = 50;
					break;
				case CoinState.Medium:
					count = 100;
					break;
				case CoinState.Big:
					count = 200;
					break;
				default:
					break;
			}

			return count;
		}
	}

	public BubbleType type;

	[SerializeField] private Sounds soundName01, explosionSound;
	[SerializeField] private float _bounceRate = 20;
	[SerializeField] private float _blinkRate = 0.15f;
	[SerializeField] private float _selfDestroyTimerRate = 4;

	private int _tubeID;
	private CoinState _state;

	private Color ColorDummy;
	private Color ColorA;
	private Color ColorB;
	private Color ColorC;

	private int _clickCount;
	private bool _selfDestroyStarted;
	private Color _color;

	private Renderer _renderer;
	private Rigidbody2D _rigidbody2D;
	private GameSettingsA _gameSettings;

	private GameObject _view;

	//----------------------------------------------------------------

	public void SetTubeID(int value)
	{
		_tubeID = value;
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

		OnDestroy?.Invoke(_tubeID);
		Destroy(gameObject);
	}

	//----------------------------------------------------------------

	[Inject]
	private void Construct(GameSettingsA gameSettings)
	{
		_gameSettings = gameSettings;
	}

	private void Awake()
	{
		_renderer = GetComponentInChildren<Renderer>();
		_rigidbody2D = GetComponentInChildren<Rigidbody2D>();
		_view = _renderer.gameObject;

		InitColors();
		Init();
	}

	private void Update()
	{
		if (_selfDestroyStarted)
		{
			_selfDestroyTimerRate -= Time.fixedDeltaTime;

			if (_selfDestroyTimerRate <= 0)
			{
				SelfDestroy();
			}
		}
	}

	private void FixedUpdate()
	{
		//GetComponent<Rigidbody2D>().velocity = transform.up * (GameController.Instance.gameSettings.moveUpSpeed /** _baseSpeedTimer*/) * Time.deltaTime;
		transform.Translate(-transform.up * (GameController.Instance.gameSettings.BubbleMoveSpeed /** _baseSpeedTimer*/) * 0.1f * Time.deltaTime);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (_selfDestroyStarted)
		{
			return;
		}

		GameController.Instance.sound.PlaySound(soundName01);

		_clickCount++;

		AddForce(_bounceRate);
		Enlarge();

		Debug.Log("click");
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		//return;

		////Very nice approach for UI objects dragging
		//transform.position = eventData.position;


		// Solution #01
		Plane plane = new Plane(Vector3.forward, transform.position);
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

		if (plane.Raycast(ray, out float distance))
		{
			transform.position = ray.origin + ray.direction * distance;
		}

		// Solution #02
		//Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
		//Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
		//Vector3 PN = -Camera.main.transform.forward; // Take current negative camera's forward as Plane's Normal
		//float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
		//Vector3 P = R.origin + R.direction * t; // Find the new point.
		//transform.position = P;
	}

	//TODO: get rid of this void
	private void InitColors()
	{
		ColorDummy = GameController.Instance.gameSettings.colorDummy;
		ColorA = GameController.Instance.gameSettings.colorA;
		ColorB = GameController.Instance.gameSettings.colorB;
		ColorC = GameController.Instance.gameSettings.colorC;
	}

	private void Init()
	{
		IsReleased = false;

		_bounceRate = GameController.Instance.gameSettings.BounceRate;
		_rigidbody2D.drag = GameController.Instance.gameSettings.DragRate;

		type = (BubbleType)UnityEngine.Random.Range(0, 3);
		_state = CoinState.Small;

		_renderer.material.color = ColorDummy;

		SetColor();

		if (GameController.Instance.gameSettings.colorMode == BubbleColorMode.Explicit)
		{
			_renderer.material.color = _color;
		}
	}

	private void SetColor()
	{
		switch (type)
		{
			case BubbleType.A: _color = ColorA; break;
			case BubbleType.B: _color = ColorB; break;
			case BubbleType.C: _color = ColorC; break;
		}
	}

	private void Enlarge()
	{
		if (_clickCount == _gameSettings.EnlargeSizeClickCount)
		{
			_view.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

			_state = CoinState.Medium;
		}
		else if (_clickCount == _gameSettings.EnlargeSizeClickCount * 2)
		{
			_view.transform.localScale = new Vector3(.7f, .7f, .7f);

			_state = CoinState.Big;

			//_startSelfDestroy = true;

			_renderer.material.color = _color;

			//StartCoroutine(BlinkRoutine());
		}
		else if (_clickCount > GameController.Instance.gameSettings.EnlargeSizeClickCount * 2)
		{
			SelfDestroy();
		}
	}

	private IEnumerator BlinkRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(_blinkRate);


			_renderer.material.color = new Color(_color.r, _color.g, _color.b, 0);

			yield return new WaitForSeconds(_blinkRate);

			_renderer.material.color = new Color(_color.r, _color.g, _color.b, 100);
		}
	}

	public class BubbleDIFactory : PlaceholderFactory<Bubble> { }
}