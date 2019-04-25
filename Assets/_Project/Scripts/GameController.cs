using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoSingleton<GameController>
{
	public static event Action OnStart;
	public static event Action OnStop;

	public GameSettings gameSettings;
	public SoundController sound;
	public Canvas canvas;

	public AudioSource audioSource;

	public GameObject coinScoreTextPref;

	private Sound bgMusic;

	GameSettingsA _gameSettingsA;
	GameSettings _gameSettingsB;

	public void StartGame()
	{
		OnStart?.Invoke();
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
	}
}
