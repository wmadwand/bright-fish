using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public static class GameProgress
	{
		static GameProgress()
		{
			MessageBus.OnLevelComplete.Receive += OnLevelComplete;
			Debug.Log("GameProgress");

		}

		private static void OnLevelComplete(Level level)
		{
			var justFinishedLevel = GameController.Instance.levelController.CurrentLevel;
			var location = GameController.Instance.levelFactory.CurrentLocation;
			var nextLevelId = GameController.Instance.levelFactory.CurrentLocation.GetNextLevelId(justFinishedLevel.ID);

			if (location.GetLevelIndex(GetMaxAvailableLevelId()) < location.GetLevelIndex(nextLevelId))
			{
				Save(nextLevelId);
			}
		}

		public static void Save(string levelId)
		{
			PlayerPrefs.SetString("Level", levelId);
		}

		public static void Load()
		{

		}

		public static void Reset()
		{
			PlayerPrefs.SetString("Level", "0101");

			MessageBus.OnGameProgressReset.Send();
		}

		public static string GetMaxAvailableLevelId()
		{
			return PlayerPrefs.GetString("Level");
		}

		public static bool InitialGameLaunch()
		{
			return !PlayerPrefs.HasKey("Level");
		}

		public static void DeleteKeyLevel()
		{
			PlayerPrefs.DeleteKey("Level");
		}
	}
}