using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Tube : MonoBehaviour
{
	//[SerializeField] private GameObject _bubblePrefab;
	[SerializeField] private Transform _bubbleSpawnPoint;

	private int _id;
	private float _randomBounceRate;
	private Bubble.BubbleDIFactory _bubbleDIFactory;
	private GameSettings _gameSettings;
	private Bubble _bubble;

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

	private void Start()
	{
		RunAfterDelay(MakeBubble);
	}

	//TODO: consider collision ignore during initialization and skip after trigger exit
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

	private void Bubble_OnDestroy(int id)
	{
		if (_id != id)
		{
			return;
		}

		RunAfterDelay(MakeBubble);
	}

	private void MakeBubble()
	{
		_bubble = _bubbleDIFactory.Create();

		_bubble.transform.SetPositionAndRotation(_bubbleSpawnPoint.position, Quaternion.identity);
		_bubble.SetParentTubeID(_id);

		_randomBounceRate = UnityEngine.Random.Range(_gameSettings.BubbleInitialBounceRate, _gameSettings.BubbleInitialBounceRate * 1.7f);
		_bubble.AddForce(_randomBounceRate);
	}

	private async void RunAfterDelay(Action callback)
	{
		float delayRate = _gameSettings.TubeBubbleThrowDelay ? UnityEngine.Random.Range(.5f, 1.5f) : 0;

		await Task.Delay(TimeSpan.FromSeconds(delayRate));
		callback();
	}

	public class TubeDIFactory : PlaceholderFactory<Tube> { }
}