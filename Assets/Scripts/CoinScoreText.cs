using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CoinScoreText : MonoBehaviour
{
	public Text text;
	public float showTime = 1.5f;

	public void SetScore(int value, bool wrongCoin = false)
	{
		text.color = wrongCoin ? Color.red : Color.white;

		var str = wrongCoin ? "-" : "";
		text.text = $"{str}{value}";
	}

	//async void Start()
	//{
	//	await Task.Delay(TimeSpan.FromSeconds(showTime));

	//	Destroy(gameObject);
	//}

	IEnumerator Start()
	{
		yield return new WaitForSeconds(showTime);

		Destroy(gameObject);
	}

}
