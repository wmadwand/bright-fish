using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum CoinType
{
	A, B, C
}

public class Coin : MonoBehaviour, IPointerClickHandler
{
	public static event Action<int> OnDestroy;

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
		type = (CoinType)UnityEngine.Random.Range(0, 3);

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

	void Enlarge()
	{
		if (_clickCount == 4)
		{
			transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
		else if (_clickCount == 8)
		{
			transform.localScale = new Vector3(2, 2, 2);

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
