using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
	public static event Action<int> OnCoinMatch;

	public CoinType type;

	public Transform scoreTextSpawnPoint;

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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().type == type)
		{
			OnCoinMatch?.Invoke(other.GetComponent<Bubble>().ScoreCount);

			SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount);
		}
		else if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().type != type)
		{
			OnCoinMatch?.Invoke(-other.GetComponent<Bubble>().ScoreCount);

			SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount, true);
		}

		other.GetComponent<Bubble>().SelfDestroy();
	}

	void SpawnCoinScroreText(int scoreCount, bool wrongCoin = false)
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(scoreTextSpawnPoint.position);

		var scoreGO = Instantiate(GameController.Instance.coinScoreTextPref, GameController.Instance.canvas.transform);
		scoreGO.transform.position = pos;

		scoreGO.GetComponent<CoinScoreText>().SetScore(scoreCount, wrongCoin);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		other.GetComponent<Bubble>().SelfDestroy();
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
