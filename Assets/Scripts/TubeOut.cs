using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

			SomeDelay(CreateCoin);
			//StartCoroutine(SomeDelayRoutine(CreateCoin));
		}
	}

	void CreateCoin()
	{

		var go = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
		go.GetComponent<Coin>().tubeId = Id;


		var randomBounceRate = UnityEngine.Random.Range(initialBounceRate, initialBounceRate * 1.7f);

		go.GetComponent<Rigidbody>().AddForce(Vector3.up * randomBounceRate, ForceMode.Impulse);
	}

	async void SomeDelay(Action callback)
	{
		var delayRate = GameController.Instance.gameSettings.delayCoinThrow ? UnityEngine.Random.Range(.5f, 1.5f) : 0;

		await Task.Delay(TimeSpan.FromSeconds(delayRate));
		callback();
	}

	//IEnumerator SomeDelayRoutine(Action callback)
	//{
	//	var delayRate = GameController.Instance.gameSettings.delayCoinThrow ? UnityEngine.Random.Range(.5f, 1.5f) : 0;

	//	yield return new WaitForSeconds(delayRate);
	//	callback();
	//}
}
