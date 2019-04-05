using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
	public GameObject coinPrefab;
	public Transform spawnPoint;

	bool isNeedCoin;

	private void Awake()
	{
		Coin.OnDestroy += Coin_OnDestroy;
	}

	private void Coin_OnDestroy()
	{
		throw new System.NotImplementedException();
	}

	private void Update()
	{
		if (isNeedCoin)
		{
			CreateCoin();
		}
	}

	void CreateCoin()
	{
		var go = Instantiate(coinPrefab, spawnPoint);
	}
}
