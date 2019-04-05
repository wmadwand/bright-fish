using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
	public GameObject coinPrefab;
	public Transform spawnPoint;

	bool isRequiredCoin = true;

	private void Awake()
	{
		Coin.OnDestroy += Coin_OnDestroy;
	}

	private void Coin_OnDestroy()
	{
		isRequiredCoin = true;
	}

	private void Update()
	{
		if (isRequiredCoin)
		{
			isRequiredCoin = false;

			CreateCoin();
		}
	}

	void CreateCoin()
	{

		var go = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
	}
}
