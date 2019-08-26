using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class UIGameMenu : MonoBehaviour
	{
		public GameObject mainScreen;
		public GameObject chooseLevelScreen;
		public GameObject howToPlayScreen;

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