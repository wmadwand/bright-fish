using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrightFish
{
	public struct LevelListItem
	{
		public string id;
		public GameObject gameObject;

		public LevelListItem(string id, GameObject gameObject)
		{
			this.id = id;
			this.gameObject = gameObject;
		}
	}

	public class UILevelList : MonoBehaviour
	{
		[SerializeField] private GameObject _levelItemTemplate;
		[SerializeField] private Transform _parent;

		private List<LevelListItem> _levelsList = new List<LevelListItem>();

		private void Awake()
		{
			MessageBus.OnGameProgressReset.Receive += OnResetGameProgress_Receive;
		}
		private void OnDestroy()
		{
			MessageBus.OnGameProgressReset.Receive -= OnResetGameProgress_Receive;
		}

		private void OnResetGameProgress_Receive()
		{
			UpdateList();
		}

		private void Start()
		{
			CreateList();
			UpdateList();
		}

		private void OnEnable()
		{
			UpdateList();
		}

		void UpdateList()
		{
			foreach (var item in _levelsList)
			{
				var location = GameController.Instance.levelFactory.CurrentLocation;

				if (location.GetLevelIndex(item.id) <= location.GetLevelIndex(GameProgress.GetCurrentLevelId()))
				{
					item.gameObject.GetComponentInChildren<Button>().interactable = true;
				}
				else
				{
					item.gameObject.GetComponentInChildren<Button>().interactable = false;
				}
			}
		}

		void CreateList()
		{
			var levels = GameController.Instance.levelFactory.CurrentLocation.Levels;

			foreach (var item in levels)
			{
				var obj = Instantiate(_levelItemTemplate, _parent);

				obj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickHandler(item.ID));
				obj.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;

				obj.GetComponentInChildren<Button>().interactable = false;

				_levelsList.Add(new LevelListItem(item.ID, obj));
			}
		}

		void OnClickHandler(string levelId)
		{
			//var levelId = GameProgress.GetCurrentLevelId();

			MessageBus.OnLevelSelected.Send(levelId);

			Debug.Log($"Start LEVEL = {levelId}");
		}
	}
}