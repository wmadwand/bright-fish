using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Tube : MonoBehaviour
{
	//[SerializeField] private GameObject _bubblePrefab;
	[SerializeField] private Transform _bubbleSpawnPoint;
	[SerializeField] private float _initialBounceRate;

	private int _id;
	private bool _isRequestedBubble = true;
	private float _randomBounceRate;
	private Bubble.BubbleDIFactory _bubbleDIFactory;
	private GameSettings _gameSettings;

	//----------------------------------------------------------------

	public void SetTubeID(int value)
	{
		_id = value;
	}

	//----------------------------------------------------------------

	[Inject]
	private void Construct(Bubble.BubbleDIFactory bubbleDIFactory, GameSettings gameSettings)
	{
		_bubbleDIFactory = bubbleDIFactory;
		_gameSettings = gameSettings;
	}

	private void Awake()
	{
		Bubble.OnDestroy += Bubble_OnDestroy;
	}

	private void Bubble_OnDestroy(int id)
	{
		if (_id != id)
		{
			return;
		}

		_isRequestedBubble = true;
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
		if (!_isRequestedBubble)
		{
			return;
		}

		_isRequestedBubble = false;
		RunAfterDelay(MakeBubble);
	}

	private void MakeBubble()
	{
		Bubble bubble = _bubbleDIFactory.Create();

		bubble.transform.SetPositionAndRotation(_bubbleSpawnPoint.position, Quaternion.identity);
		bubble.SetFactoryID(_id);

		_randomBounceRate = UnityEngine.Random.Range(_initialBounceRate, _initialBounceRate * 1.7f);
		bubble.AddForce(_randomBounceRate);
	}

	private async void RunAfterDelay(Action callback)
	{
		float delayRate = _gameSettings.BubbleThrowDelay ? UnityEngine.Random.Range(.5f, 1.5f) : 0;

		await Task.Delay(TimeSpan.FromSeconds(delayRate));
		callback();
	}

	public class TubeDIFactory : PlaceholderFactory<Tube> { }
}
