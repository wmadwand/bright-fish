using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeIn : MonoBehaviour
{
	public CoinType type;

	Color ColorA = Color.blue;
	Color ColorB = Color.yellow;
	Color ColorC = Color.green;

	Color _color;

	private Renderer _renderer;

	private void Awake()
	{
		_renderer = GetComponent<Renderer>();

		//Generate();
	}

	private void OnTriggerEnter(Collider other)
	{

	}

	private void OnTriggerExit(Collider other)
	{
		other.GetComponent<Coin>().SelfDestroy();
	}

	public void Setup(CoinType type)
	{
		this.type = type;

		switch (this.type)
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

	void Generate()
	{
		type = (CoinType)UnityEngine.Random.Range(0, 2);

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
}
