using UnityEngine;

public enum BubbleColorMode
{
	Explicit,
	Implicit
}

[CreateAssetMenu(fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
	public BubbleColorMode colorMode;

	[Header("BubbleFactory behaviour")]
	public bool BubbleThrowDelay;

	[Header("Bubble behaviour")]
	public float BubbleMoveSpeed = 10;
	public float BounceRate = 20;
	public float DragRate = 4;
	public int EnlargeSizeClickCount = 4;

	[Header("Fish color")]
	public Color colorDummy = Color.white;
	public Color colorA = Color.magenta;
	public Color colorB = Color.yellow;
	public Color colorC = Color.green;
}
