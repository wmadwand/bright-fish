using System.Collections.Generic;
using System.Linq;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class FishSpawner : MonoBehaviour
	{
		public GameObject[] SpawnPoints => _spawnPoints;

		[SerializeField] private ColorType[] _fishTypes;
		[SerializeField] private GameObject[] _spawnPoints;

		private List<Fish> _fishes = new List<Fish>();
		private System.Random _random;
		private Fish.FishDIFactory _fishDIFactory;

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

		private void OnFishRescued_Receive(Fish arg1, ColorType arg2, Vector3 arg3)
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

		private void Fish_OnHappy(Fish fish, ColorType arg1, Vector3 arg2)
		{
			CreateNewFish(fish, GetRandomBubbleType(), arg2);
		}

		private void Fish_OnDeath(Fish fish, ColorType arg1, Vector3 arg2)
		{
			CreateNewFish(fish, GetRandomBubbleType(), arg2);
		}

		private void CreateNewFish(Fish fish, ColorType arg1, Vector3 arg2)
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
			ColorType[] MyRandomArray = _fishTypes.OrderBy(x => _random.Next()).ToArray();

			for (int i = 0; i < /*2*/ MyRandomArray.Length; i++)
			{
				Spawn(MyRandomArray[i], _spawnPoints[i].transform.position);
			}
		}

		private void Spawn(ColorType bubbleType, Vector3 position)
		{
			Fish fish = _fishDIFactory.Create();


			fish.transform.SetPositionAndRotation(position, Quaternion.identity);
			fish.Setup(bubbleType);

			_fishes.Add(fish);
		}

		private ColorType GetRandomBubbleType()
		{
			return (ColorType)Random.Range(0, _fishTypes.Length);
		}
	} 
}