using System;
using UnityEngine;

public class FishFloating : MonoBehaviour
{
	public Transform _transform;

	Vector2 floatY;

	public float amplitudeY; // Set strength in Unity
	public float amplitudeX;
	float curAmpl;

	public float frequencyY = 0.1f; // Set strength in Unity
	public float frequencyX = 0.1f;

	float resFrequ;

	System.Random _random;

	private void Awake()
	{
		_random = new System.Random();
		curAmpl = amplitudeY;

		curAmpl = UnityEngine.Random.Range(.05f, amplitudeY);
		resFrequ = UnityEngine.Random.Range(.5f, frequencyY);
	}

	private void Update()
	{
		float valY = Mathf.Sin(Time.fixedTime * resFrequ) * curAmpl /*UnityEngine.Random.Range(0, amplitudeY)*/  /** (float)_random.NextDouble()*/;
		float valX = Mathf.Sin(Time.fixedTime * frequencyX) * amplitudeX /*UnityEngine.Random.Range(0, amplitudeY)*/  /** (float)_random.NextDouble()*/;

		_transform.localPosition = new Vector2(_transform.localPosition.x, valY);

		//float curPosY = (float)Math.Round(transform.localPosition.y, 3, MidpointRounding.ToEven);
		//float curBound = (float)Math.Round(curAmpl, 3, MidpointRounding.ToEven);

		//if (curPosY >= curBound /*Mathf.Approximately (curPosY, curBound)*/)
		//{
		//	curAmpl = UnityEngine.Random.Range(0, amplitudeY);
		//}
	}

	public static float NextFloat(System.Random random)
	{
		double mantissa = (random.NextDouble() * 2.0) - 1.0;
		// choose -149 instead of -126 to also generate subnormal floats (*)
		double exponent = Math.Pow(2.0, random.Next(-126, 127));
		return (float)(mantissa * exponent);
	}
}