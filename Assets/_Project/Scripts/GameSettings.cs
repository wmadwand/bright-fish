using UnityEngine;

public enum BubbleColorMode
{
	Explicit,
	Implicit
}

[CreateAssetMenu(fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
	public BubbleColorMode ColorMode => _colorMode;
	public bool TubeBubbleThrowDelay => _TubeBubbleThrowDelay;
	public float BubbleInitialBounceRate => _bubbleInitialBounceRate;

	public float BubbleMoveSpeed => _bubbleMoveSpeed;
	public float BounceRate => _bounceRate;
	public float DragRate => _dragRate;
	public int EnlargeSizeClickCount => _enlargeSizeClickCount;
	public float BlinkRate => _blinkRate;
	public bool BigBubbleSelfDestroy => _bigBubbleSelfDestroy;
	public float SelfDestroyTime => _selfDestroyTime;

	public Color ColorDummy => _colorDummy;
	public Color ColorA => _colorA;
	public Color ColorB => _colorB;
	public Color ColorC => _colorC;

	//----------------------------------------------------------------

	[Header("Common")]
	[SerializeField] private BubbleColorMode _colorMode = BubbleColorMode.Implicit;

	[Header("Tube behaviour")]
	[SerializeField] private bool _TubeBubbleThrowDelay = false;
	[SerializeField] private float _bubbleInitialBounceRate = -4;

	[Header("Bubble behaviour")]
	[SerializeField] private float _bubbleMoveSpeed = 8;
	[SerializeField] private float _bounceRate = 10;
	[SerializeField] private float _dragRate = 4;
	[SerializeField] private int _enlargeSizeClickCount = 2;
	[SerializeField] private float _blinkRate = 0.15f;
	[SerializeField] private bool _bigBubbleSelfDestroy = false;
	[SerializeField] private float _selfDestroyTime = 3;

	[Header("Fish color")]
	[SerializeField] private Color _colorDummy = Color.white;
	[SerializeField] private Color _colorA = Color.magenta;
	[SerializeField] private Color _colorB = Color.yellow;
	[SerializeField] private Color _colorC = Color.green;
}