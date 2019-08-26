using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class UIGameMenu : MonoBehaviour
	{
		public GameObject mainScreen;
		public GameObject chooseLevelScreen;
		public GameObject howToPlayScreen;

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
			MainScreen();
		}

		public void ChooseLevelScreen()
		{
			mainScreen.SetActive(false);
			chooseLevelScreen.SetActive(true);
			howToPlayScreen.SetActive(false);
		}

		public void HowToPlayScreen()
		{
			mainScreen.SetActive(false);
			chooseLevelScreen.SetActive(false);
			howToPlayScreen.SetActive(true);
		}

		public void MainScreen()
		{
			mainScreen.SetActive(true);
			chooseLevelScreen.SetActive(false);
			howToPlayScreen.SetActive(false);
		}

		public void ResetLevelList()
		{
			GameController.Instance.levelController.ResetGameProgress();
		}

		public void DeleteKeyLevel()
		{
			GameProgress.DeleteKeyLevel();
			GameController.Instance.levelController.ResetGameProgress();
		}
	}
}