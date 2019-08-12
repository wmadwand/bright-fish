using System;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	[Serializable]
	public class GameSettingsA
	{
		public BubbleColorMode colorMode;

		[Header("BubbleFactory behaviour")]
		public bool BubbleThrowDelay;

		[Header("Bubble behaviour")]
		public float BubbleMoveSpeed = 10;
		public float BounceRate = 20;
		public float DragRate = 4;
		public int EnlargeSizeClickCount = 4;

		[Header("Fish color")]
		public Color colorDummy = Color.white;
		public Color colorA = Color.magenta;
		public Color colorB = Color.yellow;
		public Color colorC = Color.green;
	}

	[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
	public class GameSettingsInstaller : ScriptableObjectInstaller
	{


		public GameSettingsA gameSettings;

		public override void InstallBindings()
		{
			Container.BindInstance(gameSettings);
		}
	} 
}