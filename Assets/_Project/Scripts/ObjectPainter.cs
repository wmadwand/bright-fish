using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

public class ObjectPainter : MonoBehaviour
{
	private SpriteRenderer[] _spriteRenderers;
	private GameSettings _gameSettings;
	private float _saturationIntence = 1;

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
		MessageBus.OnFishRescued.Receive += OnFishRescued_Receive;

		ChangeSaturation(_saturationIntence);
	}

	private void OnDestroy()
	{
		MessageBus.OnFishRescued.Receive -= OnFishRescued_Receive;
	}

	private void OnFishRescued_Receive(Fish arg1, BubbleType arg2, Vector3 arg3)
	{
		_saturationIntence -= _gameSettings.RescuedFishTargetCount * .01f;

		ChangeSaturation(_saturationIntence);
	}

	private void ChangeSaturation(float value)
	{
		foreach (var item in _spriteRenderers)
		{
			item.material.SetFloat("_EffectAmount", value);
		}
	}
}