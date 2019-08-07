using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class LevelFactory : MonoBehaviour
	{
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

		public void StartLevel(string id)
		{

		}

		private Level GetLevelSettings(string id)
		{
			return _location.GetLevel(id);
		}

		private void Create(string id)
		{
			var lvlSettings = GetLevelSettings(id);

			// SpawnTubes
			var tubes = lvlSettings.Tubes;
			SpawnTubes(tubes);

			// SpawnFishes

			// Get level built and callback =>
			//



			// Leves is ready to be launched
			MessageBus.OnLevelBuilt.Send(id);
		}

		void SpawnTubes(TubeItem[] tubeItems)
		{
			GameController.Instance.tubeSpawner.SpawnTubes(tubeItems);
		}

		void SpawnFishes()
		{

		}

	}
}