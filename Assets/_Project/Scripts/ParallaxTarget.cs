using Terminus.Game.Messages;
using UnityEngine;

public class ParallaxTarget : MonoBehaviour
{
	private void Awake()
	{
		MessageBus.OnGameStart.Receive += OnGameStart_Receive;
		MessageBus.OnGameStop.Receive += OnGameStop_Receive;
	}

	private void OnGameStop_Receive(bool obj)
	{
		GetComponent<FishFloating>().SetActive(false);
	}

	private void OnDestroy()
	{
		MessageBus.OnGameStart.Receive -= OnGameStart_Receive;
		MessageBus.OnGameStop.Receive -= OnGameStop_Receive;
	}

	private void OnGameStart_Receive()
	{
		GetComponent<FishFloating>().SetActive(true);
	}

	public void ResetPosition()
	{
		transform.position = Vector2.zero;
	}
}
