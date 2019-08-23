using System;
using UnityEngine;

public class FishFloating : MonoBehaviour
{
	public Transform _transform;

	Vector2 floatY;

	public float amplitudeY; // Set strength in Unity
	public float amplitudeX;
	float curAmplY;
	float curAmplX;

	public float frequencyY = 0.1f; // Set strength in Unity
	public float frequencyX = 0.1f;

	float resFrequY;
	float resFrequX;

	System.Random _random;

	bool _isActive;

	private void Awake()
	{
		_random = new System.Random();
		curAmplY = amplitudeY;

		curAmplY = UnityEngine.Random.Range(.05f, amplitudeY);
		curAmplX = UnityEngine.Random.Range(.05f, amplitudeX);

		resFrequY = UnityEngine.Random.Range(.5f, frequencyY);
		resFrequX = UnityEngine.Random.Range(.5f, frequencyX);
	}

	private void Update()
	{
		if (!_isActive)
		{
			return;
		}

		float valX = Mathf.Sin(Time.fixedTime * resFrequX) * curAmplX /*UnityEngine.Random.Range(0, amplitudeY)*/  /** (float)_random.NextDouble()*/;
		float valY = Mathf.Sin(Time.fixedTime * resFrequY) * curAmplY /*UnityEngine.Random.Range(0, amplitudeY)*/  /** (float)_random.NextDouble()*/;

		var xResult = (frequencyX == 0 || amplitudeX == 0) ? _transform.localPosition.x : valX;

		_transform.localPosition = new Vector2(xResult, valY);

		//float curPosY = (float)Math.Round(transform.localPosition.y, 3, MidpointRounding.ToEven);
		//float curBound = (float)Math.Round(curAmpl, 3, MidpointRounding.ToEven);

		//if (curPosY >= curBound /*Mathf.Approximately (curPosY, curBound)*/)
		//{
		//	curAmpl = UnityEngine.Random.Range(0, amplitudeY);
		//}
	}

	public void SetActive(bool value)
	{
		_isActive = value;
	}

	public static float NextFloat(System.Random random)
	{
		double mantissa = (random.NextDouble() * 2.0) - 1.0;
		// choose -149 instead of -126 to also generate subnormal floats (*)
		double exponent = Math.Pow(2.0, random.Next(-126, 127));
		return (float)(mantissa * exponent);
	}
}