using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	[SerializeField] private GameObject _gameStartPanel;
	[SerializeField] private GameObject _gameProgressPanel;
	[SerializeField] private GameObject _gameOverPanel;
	[SerializeField] private Text _finalScoreText;

	//---------------------------------------------------------

	private void Awake()
	{
		GameController.OnStart += GameController_OnGameStart;
		GameController.OnStop += GameController_OnGameStop;
	}

	private void OnDestroy()
	{
		GameController.OnStart -= GameController_OnGameStart;
		GameController.OnStop -= GameController_OnGameStop;
	}

	private void Start()
	{
		HideAllPanels();
		_gameStartPanel.SetActive(true);
	}

	private void GameController_OnGameStart()
	{
		HideAllPanels();
		_gameProgressPanel.SetActive(true);
	}

	private void GameController_OnGameStop()
	{
		ShowGameOverPanel();
	}

	private void ShowGameStartPanel()
	{
		HideAllPanels();
		_gameStartPanel.SetActive(true);
	}

	private void ShowGameOverPanel()
	{
		SetFinalScoreText();

		HideAllPanels();
		_gameOverPanel.SetActive(true);
	}

	private void ShowGameProgressPanel()
	{
		HideAllPanels();
		_gameProgressPanel.SetActive(true);
	}

	private void HideAllPanels()
	{
		_gameStartPanel.SetActive(false);
		_gameOverPanel.SetActive(false);
		_gameProgressPanel.SetActive(false);
	}

	private void SetFinalScoreText()
	{
		_finalScoreText.text = $"Your score: {ScoreManager.Instance.Score}";
	}
}