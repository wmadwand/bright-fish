using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeOut : MonoBehaviour
{
	public int Id;
	public GameObject coinPrefab;
	public Transform spawnPoint;

	bool isRequiredCoin = true;

	public float initialBounceRate;

	private void Awake()
	{
		Coin.OnDestroy += Coin_OnDestroy;
	}

	private void Coin_OnDestroy(int id)
	{
		if (Id == id)
		{
			isRequiredCoin = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{


		if (other.GetComponent<Coin>())
		{
			if (other.GetComponent<Coin>().IsReleased)
			{
				other.GetComponent<Coin>().SelfDestroy();				
			}
			else
			{
				other.GetComponent<Coin>().SetReleased();
			}

			
		}		
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
		go.GetComponent<Coin>().tubeId = Id;


		var randomBounceRate = Random.Range(initialBounceRate, initialBounceRate * 1.7f);

		go.GetComponent<Rigidbody>().AddForce(Vector3.up * randomBounceRate, ForceMode.Impulse);
	}
}
