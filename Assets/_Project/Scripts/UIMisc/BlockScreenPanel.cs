using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class BlockScreenPanel : MonoBehaviour
	{
		[SerializeField] private GameObject _blockImageGO;

		private void Awake()
		{
			MessageBus.OnGamePause.Receive += OnGamePause_Receive;

			_blockImageGO.SetActive(false);
		}

		private void OnDestroy()
		{
			MessageBus.OnGamePause.Receive -= OnGamePause_Receive;
		}

		private void OnGamePause_Receive(bool obj)
		{
			_blockImageGO.SetActive(obj);
		}
	} 
}