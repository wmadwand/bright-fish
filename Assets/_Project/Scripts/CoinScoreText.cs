using System.Collections;
using TMPro;
using UnityEngine;

public class CoinScoreText : MonoBehaviour
{
	public TextMeshProUGUI text;
	public float showTime = 1.5f;

	public void SetScore(int value, bool wrongCoin = false)
	{
		text.color = wrongCoin ? Color.red : Color.white;

		var str = wrongCoin ? "-" : "+";
		text.text = $"{str}{value}";
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds(showTime);

		//await Task.Delay(TimeSpan.FromSeconds(showTime));

		Destroy(gameObject);
	}

	//IEnumerator Start()
	//{
	//	yield return new WaitForSeconds(showTime);

	//	Destroy(gameObject);
	//}

}
