using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoSingleton<ScoreManager>
{
	public int Score { get; private set; }

	[SerializeField] private Text _scoreText;

	//---------------------------------------------------------

	private void Awake()
	{
		Fish.OnBubbleColorMatch += TubeIn_OnCoinMatch;
	}

	private void TubeIn_OnCoinMatch(int obj)
	{
		Score += obj;

		UpdateText();
	}



	private void OnDestroy()
	{
		Fish.OnBubbleColorMatch -= TubeIn_OnCoinMatch;
	}

	private void GameController_OnGameStart()
	{
		//ResetScore();
		UpdateText();
	}

	private void ResetScore()
	{
		Score = 0;
		UpdateText();
	}

	private void UpdateText()
	{
		_scoreText.text = $"Score: {Score}";
	}
}