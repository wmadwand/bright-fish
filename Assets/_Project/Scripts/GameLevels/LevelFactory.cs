using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class LevelFactory : MonoBehaviour
	{
		//public Level Level { get; private set; }

		public Location CurrentLocation => _location;

		[SerializeField] private Location _location;

		private void Awake()
		{
			MessageBus.OnLevelSelected.Receive += OnLevelSelected_Receive;
		}

		private void OnDestroy()
		{
			MessageBus.OnLevelSelected.Receive -= OnLevelSelected_Receive;
		}

		private void OnLevelSelected_Receive(string obj)
		{
			Create(obj);

			Debug.Log("OnLevelSelected_Receive");
		}

		private Level GetLevelSettings(string id)
		{
			return _location.GetLevel(id);
		}

		private void Create(string id)
		{
			var lvlSettings = GetLevelSettings(id);

			InitLevelRules(lvlSettings);

			// SpawnTubes
			var tubes = lvlSettings.Tubes;
			SpawnTubes(tubes);

			// SpawnFishes
			SpawnFishes(lvlSettings, tubes.Length);

			// Get level built and callback =>
			//
			// Leves is ready to be launched
			MessageBus.OnLevelBuilt.Send(id);
		}

		void SpawnTubes(TubeSettings[] tubeItems)
		{
			GameController.Instance.tubeSpawner.SpawnTubes(tubeItems);
		}

		void SpawnFishes(Level levelSettings, int count)
		{
			GameController.Instance.fishSpawner.Init(levelSettings);
			GameController.Instance.fishSpawner.SpawnFishes(count);
		}

		void InitLevelRules(Level level)
		{
			GameController.Instance.levelController.InitLevel(level);
		}

	}
}