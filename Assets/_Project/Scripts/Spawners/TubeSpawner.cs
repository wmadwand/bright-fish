﻿using System;
using System.Linq;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public sealed class TubeSpawner : MonoBehaviour
	{
		[SerializeField] private ColorType[] _bubbleTypes;
		private GameObject[] _tubeSpawnPoints;

		private System.Random _random;
		private Tube.TubeDIFactory _tubeDIFactory;
		private Tube[] _currentTubeCollection;

		//----------------------------------------------------------------

		[Obsolete("See SpawnTubes(TubeItem[] tubeItems)")]
		public void SpawnTubes()
		{
			_currentTubeCollection = new Tube[_bubbleTypes.Length];

			//int[] coinTypeArray = { 0, 1, 2 };
			ColorType[] randomArray = _bubbleTypes.OrderBy(x => _random.Next()).ToArray();

			for (int i = 0; i < /*1*/ randomArray.Length; i++)
			{
				Tube tube = _tubeDIFactory.Create();

				tube.transform.SetPositionAndRotation(_tubeSpawnPoints[i].transform.position, Quaternion.identity);
				//tube.SetTubeID(i);

				_currentTubeCollection[i] = tube;
			}
		}

		public void SpawnTubes(TubeSettings[] tubeItems)
		{
			_currentTubeCollection = new Tube[tubeItems.Length];

			//int[] coinTypeArray = { 0, 1, 2 };
			//ColorType[] randomArray = _bubbleTypes.OrderBy(x => _random.Next()).ToArray();

			var spawnPoints = GameAreaDesigner.GetSpawnPoints(tubeItems.Length, SpawnPointPosition.Top);

			for (int i = 0; i < /*1*/ tubeItems.Length; i++)
			{
				Tube tube = _tubeDIFactory.Create();

				tube.transform.SetPositionAndRotation(spawnPoints[i], Quaternion.identity);
				//tube.SetTubeID(i);
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
			//MessageBus.OnGameStart.Receive += GameController_OnStart;
			MessageBus.OnGameStop.Receive += GameController_OnStop;

			_random = new System.Random();
		}

		private void Start()
		{
			//SpawnTubes();
		}

		private void OnDestroy()
		{
			MessageBus.OnGameStart.Receive -= GameController_OnStart;
			MessageBus.OnGameStop.Receive -= GameController_OnStop;
		}

		private void GameController_OnStop(bool success)
		{
			DestroyTubes();
		}

		private void GameController_OnStart()
		{
			SpawnTubes();
		}
	}
}