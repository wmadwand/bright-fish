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

			// SpawnFishes

			// Get level built and callback =>
			//

			MessageBus.OnLevelBuilt.Send(id);
		}

		void SpawnTubes()
		{

		}

		void SpawnFishes()
		{

		}

	}
}