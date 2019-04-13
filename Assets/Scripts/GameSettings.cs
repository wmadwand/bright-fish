using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorMode
{
	Explicit,
	Implicit
}

[CreateAssetMenu(fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
	public ColorMode colorMode;

	[Header("TubeOut behaviour")]
	public bool delayCoinThrow;

	[Header("Coin behaviour")]
	public float moveUpSpeed = 10;
	public float bounceRate = 20;
	public float dragRate = 4;
	public int enlargeSizeClickCount = 4;
}
