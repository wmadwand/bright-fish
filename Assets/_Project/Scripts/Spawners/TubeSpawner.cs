using System;
using System.Linq;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public sealed class TubeSpawner : MonoBehaviour
	{
		//[SerializeField] private ColorType[] _bubbleTypes;
		private GameObject[] _tubeSpawnPoints;

		private System.Random _random;
		private Tube.TubeDIFactory _tubeDIFactory;
		private Tube[] _currentTubeCollection;

		//----------------------------------------------------------------

		[Obsolete("See SpawnTubes(TubeItem[] tubeItems)")]
		public void SpawnTubes()
		{
			//_currentTubeCollection = new Tube[_bubbleTypes.Length];
			//ColorType[] randomArray = _bubbleTypes.OrderBy(x => _random.Next()).ToArray();

			//for (int i = 0; i < randomArray.Length; i++)
			//{
			//	Tube tube = _tubeDIFactory.Create();

			//	tube.transform.SetPositionAndRotation(_tubeSpawnPoints[i].transform.position, Quaternion.identity);

			//	_currentTubeCollection[i] = tube;
			//}
		}

		public void SpawnTubes(TubeSettings[] tubeItems)
		{
			_currentTubeCollection = new Tube[tubeItems.Length];
			var spawnPoints = GameAreaDesigner.GetSpawnPoints(tubeItems.Length, SpawnPointPosition.Top);

			for (int i = 0; i < tubeItems.Length; i++)
			{
				Tube tube = _tubeDIFactory.Create();

				tube.transform.SetPositionAndRotation(spawnPoints[i], Quaternion.identity);
				tube.Init(i, tubeItems[i]);

				_currentTubeCollection[i] = tube;
			}
		}

		public void DestroyTubes()
		{
			Array.ForEach(_currentTubeCollection, item => item.SelfDestroy());
		}

		//----------------------------------------------------------------

		[Inject]
		private void Construct(Tube.TubeDIFactory tubeDIFactory)
		{
			_tubeDIFactory = tubeDIFactory;
		}

		private void Awake()
		{
			MessageBus.OnGameStop.Receive += GameController_OnStop;

			_random = new System.Random();
		}

		private void OnDestroy()
		{
			MessageBus.OnGameStop.Receive -= GameController_OnStop;
		}

		private void GameController_OnStop(bool success)
		{
			DestroyTubes();
		}
	}
}