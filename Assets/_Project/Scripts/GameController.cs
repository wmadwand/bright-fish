using System;
using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : MonoSingleton<GameController>
{
	public bool IsGameActive { get; private set; }

	//public static event Action OnStart;
	//public static event Action<bool> OnStop;

	public GameSettings gameSettings;
	public SoundController sound;
	public Canvas canvas;

	public AudioSource audioSource;

	public GameObject coinScoreTextPref;

	private Sound bgMusic;

	GameSettingsA _gameSettingsA;
	GameSettings _gameSettingsB;

	private void Awake()
	{


		MessageBus.OnPlayerLivesOut.Receive += LiveController_OnLivesOut;
		MessageBus.OnLevelComplete.Receive += LevelController_OnLevelComplete;
	}

	private void LevelController_OnLevelComplete()
	{
		IsGameActive = false;

		MessageBus.OnGameStop.Send(true);
	}

	private void LiveController_OnLivesOut()
	{
		IsGameActive = false;

		MessageBus.OnGameStop.Send(true);
	}

	public void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		StartGame();
	}

	public void StartGame()
	{
		IsGameActive = true;

		MessageBus.OnGameStart.Send();
	}

	private void PlayBgMusic()
	{
		Sound bgMusic = sound.SoundLibrary.Data.Find(item => item.name == Sounds.backgroundMusic);
		audioSource.volume = bgMusic.volume;
		audioSource.clip = bgMusic.audioClip;
		audioSource.Play();
	}

	[Inject]
	private void Construct(GameSettingsA gameSettingsA, GameSettings gameSettingsB)
	{
		_gameSettingsA = gameSettingsA;
		_gameSettingsB = gameSettingsB;
	}

	private void Start()
	{
		//InitGameStuff();
		PlayBgMusic();

		//StartGame();
	}

	private void OnDestroy()
	{
		MessageBus.OnPlayerLivesOut.Receive -= LiveController_OnLivesOut;
		MessageBus.OnLevelComplete.Receive -= LevelController_OnLevelComplete;
	}
}
