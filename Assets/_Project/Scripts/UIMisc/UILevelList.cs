using System.Collections.Generic;
using Terminus.Game.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrightFish
{
	public class UILevelList : MonoBehaviour
	{
		[SerializeField] private GameObject _levelItemTemplate;
		[SerializeField] private Transform _parent;

		private List<LevelListItem> _levelsList = new List<LevelListItem>();

		//----------------------------------------------------------------

		private void Awake()
		{
			MessageBus.OnGameProgressReset.Receive += OnResetGameProgress_Receive;
		}

		private void OnEnable()
		{
			UpdateList();
		}

		private void Start()
		{
			CreateList();
			UpdateList();
		}

		private void OnDestroy()
		{
			MessageBus.OnGameProgressReset.Receive -= OnResetGameProgress_Receive;
		}

		private void OnResetGameProgress_Receive()
		{
			UpdateList();
		}

		private void UpdateList()
		{
			foreach (var item in _levelsList)
			{
				var location = GameController.Instance.levelFactory.CurrentLocation;

				if (location.GetLevelIndex(item.id) <= location.GetLevelIndex(GameProgress.GetMaxAvailableLevelId()) && location.GetLevelIndex(item.id) >= 0)
				{
					item.button.interactable = true;
				}
				else
				{
					//item.button.interactable = false;
				}

				item.button.interactable = true;

				item.gameObject.SetActive(true);
			}
		}

		private void CreateList()
		{
			var levels = GameController.Instance.levelFactory.CurrentLocation.Levels;

			foreach (var item in levels)
			{
				var obj = Instantiate(_levelItemTemplate, _parent);

				obj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickHandler(item.ID));
				obj.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;

				obj.GetComponentInChildren<Button>().interactable = false;

				obj.SetActive(false);

				_levelsList.Add(new LevelListItem(item.ID, obj.GetComponentInChildren<Button>(), obj));
			}

			AddDummyLevels();
		}

		private void AddDummyLevels()
		{
			for (int i = _levelsList.Count + 1; i < 16; i++)
			{
				var obj = Instantiate(_levelItemTemplate, _parent);

				obj.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

				obj.GetComponentInChildren<Button>().interactable = false;

				obj.SetActive(false);

				_levelsList.Add(new LevelListItem(i.ToString(), obj.GetComponentInChildren<Button>(), obj));

			}
		}

		private void OnClickHandler(string levelId)
		{
			//var levelId = GameProgress.GetCurrentLevelId();

			MessageBus.OnLevelSelected.Send(levelId);

			Debug.Log($"Start LEVEL = {levelId}");
		}

		//----------------------------------------------------------------

		public struct LevelListItem
		{
			public string id;
			public Button button;
			public GameObject gameObject;

			public LevelListItem(string id, Button button, GameObject gameObject)
			{
				this.id = id;
				this.button = button;
				this.gameObject = gameObject;
			}
		}
	}
}