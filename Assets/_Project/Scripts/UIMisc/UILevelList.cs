using System.Collections;
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

		private void Awake()
		{

		}

		private void Start()
		{
			CreateList();
		}

		void CreateList()
		{
			var levels = GameController.Instance.levelFactory.CurrentLocation.Levels;

			foreach (var item in levels)
			{
				var obj = Instantiate(_levelItemTemplate, _parent);

				obj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickHandler(item.ID));
				obj.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
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