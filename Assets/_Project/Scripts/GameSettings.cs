using UnityEngine;

public enum BubbleColorMode
{
	Explicit,
	Implicit
}

[CreateAssetMenu(fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
	[SerializeField] private BubbleColorMode _colorMode = BubbleColorMode.Implicit;
	public BubbleColorMode ColorMode => _colorMode;

	[Header("Tube behaviour")]

	[SerializeField] private bool _bubbleThrowDelay = false;
	public bool BubbleThrowDelay => _bubbleThrowDelay;

	[SerializeField] private float _bubbleInitialBounceRate = -4;
	public float BubbleInitialBounceRate => _bubbleInitialBounceRate;


	[Header("Bubble behaviour")]

	[SerializeField] private float _bubbleMoveSpeed = 8;
	public float BubbleMoveSpeed => _bubbleMoveSpeed;

	[SerializeField] private float _bounceRate = 10;
	public float BounceRate => _bounceRate;

	[SerializeField] private float _dragRate = 4;
	public float DragRate => _dragRate;

	[SerializeField] private int _enlargeSizeClickCount = 2;
	public int EnlargeSizeClickCount => _enlargeSizeClickCount;

	[SerializeField] private float _blinkRate =  0.15f;
	public float BlinkRate => _blinkRate;

	[SerializeField] private bool _bigBubbleSelfDestroy =false;
	public bool BigBubbleSelfDestroy => _bigBubbleSelfDestroy;

	[SerializeField] private float _selfDestroyTimeRate = 3;
	public float SelfDestroyTimeRate => _selfDestroyTimeRate;


	[Header("Fish color")]

	[SerializeField] private Color _colorDummy = Color.white;
	public Color ColorDummy => _colorDummy;

	[SerializeField] private Color _colorA = Color.magenta;
	public Color ColorA => _colorA;

	[SerializeField] private Color _colorB = Color.yellow;
	public Color ColorB => _colorB;

	[SerializeField] private Color _colorC = Color.green;
	public Color ColorC => _colorC;
}