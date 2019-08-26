using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class ObjectPainter : MonoBehaviour
	{
		private SpriteRenderer[] _spriteRenderers;
		private GameSettings _gameSettings;
		private float _saturationIntence = 1;

		//----------------------------------------------------------------

		[Inject]
		private void Construct(GameSettings gameSettings)
		{
			_gameSettings = gameSettings;
		}

		private void Awake()
		{
			_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		}

		private void Start()
		{
			MessageBus.OnFishFinishedSmiling.Receive += OnFishFinishedSmiling;
			MessageBus.OnLocationPaint.Receive += OnLocationPaint_Receive;

			ChangeSaturation(_saturationIntence);
		}

		private void OnLocationPaint_Receive(float obj)
		{
			ChangeSaturation(obj);
		}

		private void OnDestroy()
		{
			MessageBus.OnFishFinishedSmiling.Receive -= OnFishFinishedSmiling;
			MessageBus.OnLocationPaint.Receive -= OnLocationPaint_Receive;
		}

		private void OnFishFinishedSmiling(Fish arg1, ColorType arg2, Vector3 arg3)
		{
			//_saturationIntence -= GameController.Instance.levelController.CurrentLevel.RescueFishTargetCount * .01f;

			//ChangeSaturation(_saturationIntence);
		}

		private void ChangeSaturation(float value)
		{
			foreach (var item in _spriteRenderers)
			{
				item.material.SetFloat("_EffectAmount", value);
			}
		}
	}
}