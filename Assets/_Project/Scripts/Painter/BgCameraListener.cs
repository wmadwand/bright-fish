using DigitalRuby.SimpleLUT;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class BgCameraListener : MonoBehaviour
	{
		private SimpleLUT _simpleLUT;
		private GameSettings _gameSettings;

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Awake()
		{
			_simpleLUT = GetComponent<SimpleLUT>();
			_simpleLUT.Saturation = -1;
		}

		private void Start()
		{
			MessageBus.OnFishRescued.Receive += OnFishRescued_Receive;
		}

		private void OnFishRescued_Receive(Fish arg1, ColorType arg2, Vector3 arg3)
		{
			_simpleLUT.Saturation += _gameSettings.RescuedFishTargetCount * .01f;
		}

		private void OnDestroy()
		{
			MessageBus.OnFishRescued.Receive -= OnFishRescued_Receive;
		}
	} 
}
