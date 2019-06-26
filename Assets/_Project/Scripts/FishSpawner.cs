using System.Collections.Generic;
using System.Linq;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

public class FishSpawner : MonoBehaviour
{
	public GameObject[] SpawnPoint => _spawnPoints;

	[SerializeField] private BubbleType[] _fishTypes;
	[SerializeField] private GameObject[] _spawnPoints;

	private System.Random _random;
	private Fish.FishDIFactory _fishDIFactory;

	private List<Fish> _fishes = new List<Fish>();

	//----------------------------------------------------------------

	[Inject]
	private void Construct(Fish.FishDIFactory fishDIFactory)
	{
		_fishDIFactory = fishDIFactory;
	}

	private void Awake()
	{
		_random = new System.Random();

		MessageBus.OnFishDead.Receive += Fish_OnDeath;
		MessageBus.OnFishFinishedSmiling.Receive += Fish_OnHappy;
		MessageBus.OnFishRescued.Receive += OnFishRescued_Receive;

		MessageBus.OnGameStart.Receive += GameController_OnStart;
		MessageBus.OnGameStop.Receive += GameController_OnStop;
	}

	private void Start()
	{
		//InitSpawn();
	}

	private void OnDestroy()
	{
		MessageBus.OnFishDead.Receive -= Fish_OnDeath;
		MessageBus.OnFishFinishedSmiling.Receive -= Fish_OnHappy;
		MessageBus.OnFishRescued.Receive -= OnFishRescued_Receive;

		MessageBus.OnGameStart.Receive -= GameController_OnStart;
		MessageBus.OnGameStop.Receive -= GameController_OnStop;
	}

	private void OnFishRescued_Receive(Fish arg1, BubbleType arg2, Vector3 arg3)
	{

	}

	private void GameController_OnStop(bool success)
	{
		_fishes.ForEach(fish => fish.Destroy());
		_fishes.Clear();
	}

	private void GameController_OnStart()
	{
		InitSpawn();
	}

	private void Fish_OnHappy(Fish fish, BubbleType arg1, Vector3 arg2)
	{
		_fishes.Remove(fish);

		if (GameController.Instance.IsGameActive)
		{
			Spawn(arg1, arg2);
		}
	}

	private void Fish_OnDeath(Fish fish, BubbleType arg1, Vector3 arg2)
	{

		_fishes.Remove(fish);

		if (GameController.Instance.IsGameActive)
		{
			Spawn(arg1, arg2);
		}
	}

	private void InitSpawn()
	{
		//int[] coinTypeArray = { 0, 1, 2 };
		BubbleType[] MyRandomArray = _fishTypes.OrderBy(x => _random.Next()).ToArray();

		for (int i = 0; i < /*2*/ MyRandomArray.Length; i++)
		{
			Fish fish = _fishDIFactory.Create();
			fish.transform.SetPositionAndRotation(_spawnPoints[i].transform.position, Quaternion.identity);

			fish.Setup(MyRandomArray[i]);

			_fishes.Add(fish);
		}
	}

	private void Spawn(BubbleType coinType, Vector3 position)
	{
		Fish fish = _fishDIFactory.Create();
		fish.transform.SetPositionAndRotation(position, Quaternion.identity);

		fish.Setup((BubbleType)Random.Range(0, _fishTypes.Length)/*coinType*/);

		_fishes.Add(fish);
	}
}