using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum CoinType
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
					count = 10;
					break;
				case CoinState.Medium:
					count = 100;
					break;
				case CoinState.Big:
					count = 500;
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

	public CoinType type;

	Color ColorA = Color.blue;
	Color ColorB = Color.yellow;
	Color ColorC = Color.green;

	int _clickCount;
	bool _startSelfDestroy;
	float _countdownRate = 4;
	Color _color;

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
		_renderer = GetComponent<Renderer>();

		Init();
	}

	private void Start()
	{
		//StartCoroutine(BlinkRoutine());
	}

	void Init()
	{
		type = (CoinType)UnityEngine.Random.Range(0, 2);
		_state = CoinState.Small;

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

		_renderer.material.color = _color;
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
		if (_clickCount == 4)
		{
			transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

			_state = CoinState.Medium;
		}
		else if (_clickCount == 8)
		{
			transform.localScale = new Vector3(2, 2, 2);

			_state = CoinState.Big;

			_startSelfDestroy = true;
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
