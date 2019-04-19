using System;
using System.Threading.Tasks;
using UnityEngine;

public class BubbleFactory : MonoBehaviour
{
	public int Id;
	public GameObject coinPrefab;
	public Transform spawnPoint;

	private bool isRequiredCoin = true;

	public float initialBounceRate;

	private float _randomBounceRate;

	//----------------------------------------------------------------

	private void Awake()
	{
		Bubble.OnDestroy += Coin_OnDestroy;
	}

	private void Coin_OnDestroy(int id)
	{
		if (Id == id)
		{
			isRequiredCoin = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Bubble>())
		{
			if (other.GetComponent<Bubble>().IsReleased)
			{
				other.GetComponent<Bubble>().SelfDestroy();
			}
			else
			{
				other.GetComponent<Bubble>().SetReleased();
			}


		}
	}

	private void Update()
	{
		if (isRequiredCoin)
		{
			isRequiredCoin = false;

			SomeDelay(MakeBubble);
		}
	}

	private void MakeBubble()
	{
		GameObject go = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
		Bubble bubble = go.GetComponent<Bubble>();
		bubble.SetFactoryID(Id);

		_randomBounceRate = UnityEngine.Random.Range(initialBounceRate, initialBounceRate * 1.7f);
		bubble.AddForce(_randomBounceRate);
	}

	private async void SomeDelay(Action callback)
	{
		var delayRate = GameController.Instance.gameSettings.BubbleThrowDelay ? UnityEngine.Random.Range(.5f, 1.5f) : 0;

		await Task.Delay(TimeSpan.FromSeconds(delayRate));
		callback();
	}
}
