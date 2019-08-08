using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public static class GameProgress
	{
		static GameProgress()
		{
			MessageBus.OnLevelComplete.Receive += OnLevelComplete;
		}

		private static void OnLevelComplete()
		{
			var nextLevelId = GameController.Instance.levelFactory.CurrentLocation.GetNextLevelId(GetCurrentLevelId());
			Save(nextLevelId);
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
		}

		public static string GetCurrentLevelId()
		{
			return PlayerPrefs.GetString("Level");
		}
	}
}