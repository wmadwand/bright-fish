using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bubble : MonoBehaviour, IPointerClickHandler, IDragHandler
{
	public static event Action<int> OnDestroy;

	[SerializeField] Sounds soundName01, explosionSound;

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

	private CoinState _state;

	[SerializeField] float _bounceRate = 20;
	[SerializeField] float _blinkRate = 0.15f;

	//TODO: make it private
	public int _tubeId { get; set; }

	public CoinType type;

	private Color ColorDummy;
	private Color ColorA;
	private Color ColorB;
	private Color ColorC;

	private int _clickCount;
	private bool _startSelfDestroy;
	private float _countdownRate = 4;
	private Color _color;

	public bool IsReleased { get; private set; }

	private Renderer _renderer;
	private Rigidbody2D _rigidbody2D;

	private GameObject _view;

	//----------------------------------------------------------------

	public void SetFactoryID(int value)
	{
		_tubeId = value;
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

		OnDestroy?.Invoke(_tubeId);

		Destroy(gameObject);
	}

	//----------------------------------------------------------------

	private void Awake()
	{
		_renderer = GetComponentInChildren<Renderer>();
		_rigidbody2D = GetComponentInChildren<Rigidbody2D>();

		SetColors();
		Init();

		_view = GetComponentInChildren<Renderer>().gameObject;
	}

	private void Start()
	{
		//StartCoroutine(BlinkRoutine());
	}

	private void Update()
	{
		if (_startSelfDestroy)
		{
			_countdownRate -= Time.fixedDeltaTime;

			if (_countdownRate <= 0)
			{
				SelfDestroy();
			}
		}
	}

	private void FixedUpdate()
	{
		//GetComponent<Rigidbody2D>().velocity = transform.up * (GameController.Instance.gameSettings.moveUpSpeed /** _baseSpeedTimer*/) * Time.deltaTime;
		transform.Translate(-transform.up * (GameController.Instance.gameSettings.moveUpSpeed /** _baseSpeedTimer*/) * 0.1f * Time.deltaTime);
	}

	private void SetColors()
	{
		ColorDummy = GameController.Instance.gameSettings.colorDummy;
		ColorA = GameController.Instance.gameSettings.colorA;
		ColorB = GameController.Instance.gameSettings.colorB;
		ColorC = GameController.Instance.gameSettings.colorC;
	}

	private void Init()
	{
		IsReleased = false;

		_bounceRate = GameController.Instance.gameSettings.bounceRate;
		GetComponent<Rigidbody2D>().drag = GameController.Instance.gameSettings.dragRate;


		type = (CoinType)UnityEngine.Random.Range(0, 3);
		_state = CoinState.Small;

		_renderer.material.color = ColorDummy;

		switch (type)
		{
			case CoinType.A:
				_color = ColorA;
				break;
			case CoinType.B:
				_color = ColorB;
				break;
			case CoinType.C:
				_color = ColorC;
				break;
			default:
				break;
		}

		if (GameController.Instance.gameSettings.colorMode == ColorMode.Explicit)
		{
			_renderer.material.color = _color;
		}

		//_renderer.material.color = _color;
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (_startSelfDestroy)
		{
			return;
		}

		GameController.Instance.sound.PlaySound(soundName01);

		_clickCount++;

		GetComponent<Rigidbody2D>().AddForce(Vector3.up * _bounceRate, ForceMode2D.Impulse);

		Enlarge();

		Debug.Log("click");
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		//return;

		////Very nice approach for 2D objects dragging
		//transform.position = eventData.position;


		// Solution #01
		Plane plane = new Plane(Vector3.forward, transform.position);
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

		if (plane.Raycast(ray, out float distamce))
		{
			transform.position = ray.origin + ray.direction * distamce;
		}

		// Solution #02
		//Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
		//Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
		//Vector3 PN = -Camera.main.transform.forward; // Take current negative camera's forward as Plane's Normal
		//float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
		//Vector3 P = R.origin + R.direction * t; // Find the new point.

		//transform.position = P;
	}

	private void Enlarge()
	{
		if (_clickCount == GameController.Instance.gameSettings.enlargeSizeClickCount)
		{
			_view.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

			_state = CoinState.Medium;
		}
		else if (_clickCount == GameController.Instance.gameSettings.enlargeSizeClickCount * 2)
		{
			_view.transform.localScale = new Vector3(.7f, .7f, .7f);

			_state = CoinState.Big;

			//_startSelfDestroy = true;

			_renderer.material.color = _color;

			//StartCoroutine(BlinkRoutine());
		}
		else
		{
			SelfDestroy();
		}
	}

	private void Blink()
	{

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
}
