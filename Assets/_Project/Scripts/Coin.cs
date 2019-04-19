using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum BubbleType
{
	A, B, C
}

public enum CoinState
{
	Small, Medium, Big
}

public class Coin : MonoBehaviour, IPointerClickHandler, IDragHandler
{
	public static event Action<int> OnDestroy;

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

	public float bounceRate = 20;
	public float blinkRate = 0.15f;

	public int tubeId;

	public BubbleType type;

	Color ColorDummy;
	Color ColorA;
	Color ColorB;
	Color ColorC;

	int _clickCount;
	bool _startSelfDestroy;
	float _countdownRate = 4;
	Color _color;

	public bool IsReleased { get; private set; }

	private Renderer _renderer;

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

	private void Awake()
	{
		_renderer = GetComponentInChildren<Renderer>();

		SetColors();
		Init();
	}

	private void Start()
	{
		//StartCoroutine(BlinkRoutine());
	}

	public void SetReleased()
	{
		IsReleased = true;
	}

	void SetColors()
	{
		 ColorDummy = GameController.Instance.gameSettings.colorDummy;
		 ColorA = GameController.Instance.gameSettings.colorA;
		 ColorB = GameController.Instance.gameSettings.colorB;
		 ColorC = GameController.Instance.gameSettings.colorC;
	}

	void Init()
	{
		IsReleased = false;

		bounceRate = GameController.Instance.gameSettings.BounceRate;
		GetComponent<Rigidbody>().drag = GameController.Instance.gameSettings.DragRate;


		type = (BubbleType)UnityEngine.Random.Range(0, 3);
		_state = CoinState.Small;

		_renderer.material.color = ColorDummy;

		switch (type)
		{
			case BubbleType.A:
				_color = ColorA;
				break;
			case BubbleType.B:
				_color = ColorB;
				break;
			case BubbleType.C:
				_color = ColorC;
				break;
			default:
				break;
		}

		if (GameController.Instance.gameSettings.colorMode == BubbleColorMode.Explicit)
		{
			_renderer.material.color = _color;
		}

		//_renderer.material.color = _color;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_startSelfDestroy)
		{
			return;
		}

		_clickCount++;

		GetComponent<Rigidbody>().AddForce(Vector3.up * bounceRate, ForceMode.Impulse);

		Enlarge();

		Debug.Log("click");
	}

	public void OnDrag(PointerEventData eventData)
	{
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

	void Enlarge()
	{
		if (_clickCount == GameController.Instance.gameSettings.EnlargeSizeClickCount)
		{
			transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

			_state = CoinState.Medium;
		}
		else if (_clickCount == GameController.Instance.gameSettings.EnlargeSizeClickCount * 2)
		{
			transform.localScale = new Vector3(2, 2, 2);

			_state = CoinState.Big;

			_startSelfDestroy = true;

			_renderer.material.color = _color;

			StartCoroutine(BlinkRoutine());
		}
	}

	public void SelfDestroy()
	{
		OnDestroy?.Invoke(tubeId);

		Destroy(gameObject);
	}

	void Blink()
	{

	}

	IEnumerator BlinkRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(blinkRate);


			_renderer.material.color = new Color(_color.r, _color.g, _color.b, 0);

			yield return new WaitForSeconds(blinkRate);

			_renderer.material.color = new Color(_color.r, _color.g, _color.b, 100);
		}
	}

}
