using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public static class LocationPaintProgress
	{
		public static float CurrentPaintValue => PlayerPrefs.GetFloat("LocationPaintValue");

		static LocationPaintProgress()
		{
			MessageBus.OnLevelComplete.Receive += OnLevelComplete;
			MessageBus.OnGameProgressReset.Receive += OnGameProgressReset_Receive;

			Debug.Log("LocationPaintProgress");
		}

		public static void UpdatePaintLocation()
		{
			MessageBus.OnLocationPaint.Send(CurrentPaintValue);
		}

		private static void OnGameProgressReset_Receive()
		{
			Reset();
		}

		private static void OnLevelComplete(Level lvl)
		{
			var newValue = CurrentPaintValue - lvl.PaintRateInput;

			MessageBus.OnLocationPaint.Send(newValue);

			Save(newValue);
		}

		public static void Save(float value)
		{
			PlayerPrefs.SetFloat("LocationPaintValue", value);
		}

		private static void Reset()
		{
			PlayerPrefs.SetFloat("LocationPaintValue", 1);
			MessageBus.OnLocationPaint.Send(CurrentPaintValue);
		}
	}
}