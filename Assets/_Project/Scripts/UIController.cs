using Terminus.Game.Messages;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	[SerializeField] private GameObject _gameStartPanel;
	[SerializeField] private GameObject _gameProgressPanel;
	[SerializeField] private GameObject _gameOverPanelFail;
	[SerializeField] private GameObject _gameOverPanelSuccess;
	[SerializeField] private Text _finalScoreText;

	//---------------------------------------------------------

	private void Awake()
	{
		//GameController.OnStart += GameController_OnGameStart;
		//GameController.OnStop += GameController_OnGameStop;

		MessageBus.OnGameStart.Receive += GameController_OnGameStart;
		MessageBus.OnGameStop.Receive += GameController_OnGameStop;
	}

	private void OnDestroy()
	{
		//GameController.OnStart -= GameController_OnGameStart;
		//GameController.OnStop -= GameController_OnGameStop;
	}

	private void Start()
	{
		HideAllPanels();
		//_gameStartPanel.SetActive(true);
	}

	private void GameController_OnGameStart()
	{
		HideAllPanels();
		_gameProgressPanel.SetActive(true);
	}

	private void GameController_OnGameStop(bool success)
	{
		ShowGameOverPanel(success);
	}

	private void ShowGameStartPanel()
	{
		HideAllPanels();
		_gameStartPanel.SetActive(true);
	}

	private void ShowGameOverPanel(bool success)
	{
		SetFinalScoreText();

		HideAllPanels();

		var panel = success ? _gameOverPanelSuccess : _gameOverPanelFail;
		panel.SetActive(true);
	}

	private void ShowGameProgressPanel()
	{
		HideAllPanels();
		_gameProgressPanel.SetActive(true);
	}

	private void HideAllPanels()
	{
		_gameStartPanel.SetActive(false);
		_gameOverPanelFail.SetActive(false);
		_gameProgressPanel.SetActive(false);
		_gameOverPanelSuccess.SetActive(false);
	}

	private void SetFinalScoreText()
	{
		_finalScoreText.text = $"Your score: {ScoreManager.Instance.Score}";
	}
}