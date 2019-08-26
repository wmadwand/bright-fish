using System;
using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BrightFish
{
	public class GameController : MonoSingleton<GameController>
	{
		public bool IsGameActive { get; private set; }

		public GameSettings gameSettings;
		public SoundController sound;
		public FishSpawner fishSpawner;
		public TubeSpawner tubeSpawner;
		public LevelController levelController;
		public LevelFactory levelFactory;
		public Canvas canvas;

		public AudioSource audioSource;

		public GameObject coinScoreTextPref;

		private Sound bgMusic;

		GameSettingsA _gameSettingsA;
		GameSettings _gameSettingsB;

		//----------------------------------------------------------------

		//public void StartGame()
		//{
		//	IsGameActive = true;

		//	MessageBus.OnGameStart.Send();
		//}

		public void StartGame()
		{
			var levelId = GameProgress.GetMaxAvailableLevelId();

			MessageBus.OnLevelSelected.Send(levelId);
		}

		public void ResetScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			//StartGame();
		}

		//----------------------------------------------------------------

		[Inject]
		private void Construct(GameSettingsA gameSettingsA, GameSettings gameSettingsB)
		{
			_gameSettingsA = gameSettingsA;
			_gameSettingsB = gameSettingsB;
		}

		private void Awake()
		{
			MessageBus.OnPlayerLivesOut.Receive += LiveController_OnLivesOut;
			MessageBus.OnLevelComplete.Receive += LevelController_OnLevelComplete;
			MessageBus.OnLevelBuilt.Receive += OnLevelBuilt_Receive;
		}

		private void OnLevelBuilt_Receive(string obj)
		{
			IsGameActive = true;

			MessageBus.OnGameStart.Send();
		}

		private void Start()
		{
			PlayBgMusic();

			if (GameProgress.InitialGameLaunch())
			{
				GameProgress.Reset();
			}
		}

		private void OnDestroy()
		{
			MessageBus.OnPlayerLivesOut.Receive -= LiveController_OnLivesOut;
			MessageBus.OnLevelComplete.Receive -= LevelController_OnLevelComplete;
			MessageBus.OnLevelBuilt.Receive -= OnLevelBuilt_Receive;
		}

		private void LevelController_OnLevelComplete(Level level)
		{
			IsGameActive = false;

			MessageBus.OnGameStop.Send(true);
		}

		private void LiveController_OnLivesOut()
		{
			IsGameActive = false;

			MessageBus.OnGameStop.Send(true);
		}

		private void PlayBgMusic()
		{
			Sound bgMusic = sound.SoundLibrary.Data.Find(item => item.name == Sounds.backgroundMusic);
			audioSource.volume = bgMusic.volume;
			audioSource.clip = bgMusic.audioClip;
			audioSource.Play();
		}
	}
}
