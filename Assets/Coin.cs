using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour, IPointerClickHandler
{
	public static event Action OnDestroy;
	public float bounceRate = 20;
	public float blinkRate = 0.15f;

	int _clickCount;
	bool _startSelfDestroy;
	float _countdownRate = 4;

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
	}

	private void Start()
	{
		//StartCoroutine(BlinkRoutine());
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
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if (_clickCount == 8)
		{
			transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

			_startSelfDestroy = true;
			StartCoroutine(BlinkRoutine());
		}
	}

	void SelfDestroy()
	{
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

			_renderer.material.color = new Color(255, 208, 0, 0);

			yield return new WaitForSeconds(blinkRate);

			_renderer.material.color =  new Color32(255, 208, 0, 100);
		}
	}

}
