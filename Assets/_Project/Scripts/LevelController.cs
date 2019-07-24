using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BrightFish
{
	public class LevelController : MonoBehaviour
	{
		[SerializeField] private Text _rescuedFishText;

		private int _rescuedFishTargetCount;
		private int _rescuedFishCurrentCount;
		private GameSettings _gameSettings;

		public void ResetLevel()
		{
			_rescuedFishTargetCount = _gameSettings.RescuedFishTargetCount;
			_rescuedFishCurrentCount = 0;

			UpdateText();
		}

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Awake()
		{
			ResetLevel();

			MessageBus.OnFishRescued.Receive += OnFishRescued;
			MessageBus.OnPlayerLivesOut.Receive += LiveController_OnLivesOut;
			MessageBus.OnGameStart.Receive += GameController_OnStart;
		}

		private void GameController_OnStart()
		{
			ResetLevel();
		}

		private void LiveController_OnLivesOut()
		{
			MessageBus.OnLevelFailed.Send();
		}

		private void OnFishRescued(Fish fish, ColorType arg1, Vector3 arg2)
		{
			_rescuedFishCurrentCount++;
			UpdateText();

			if (_rescuedFishCurrentCount >= _rescuedFishTargetCount)
			{
				MessageBus.OnLevelComplete.Send();
			}
		}

		private void OnDestroy()
		{
			MessageBus.OnFishRescued.Receive -= OnFishRescued;
			MessageBus.OnPlayerLivesOut.Receive -= LiveController_OnLivesOut;
			MessageBus.OnGameStart.Receive -= GameController_OnStart;
		}

		void UpdateText()
		{
			_rescuedFishText.text = $"{_rescuedFishCurrentCount}/{_rescuedFishTargetCount}";
		}
	} 
}