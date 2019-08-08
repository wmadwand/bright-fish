using System;
using UnityEngine;

public enum BubbleColorMode
{
	Explicit,
	Implicit
}

[CreateAssetMenu(fileName = "GameSettings", menuName = "BrightFish/GameSettings")]
public class GameSettings : ScriptableObject
{
	public bool GodMode => _godMode;

	public BubbleColorMode ColorMode => _colorMode;
	public int LivesCount => _livesCount;
	//public int RescuedFishTargetCount => _rescuedFishTargetCount;

	//public bool TubeBubbleThrowDelay => _TubeBubbleThrowDelay;
	//public float BubbleInitialBounceRate => _bubbleInitialBounceRate;

	public float BubbleMoveSpeed => _bubbleMoveSpeed;
	public float BounceRate => _bounceRate;
	public float DragRate => _dragRate;
	public int EnlargeSizeClickCount => _enlargeSizeClickCount;
	public ClickEnlargeSizePair[] ClickEnlargeSizePairs => _clickEnlargeSizePairs;
	public float BlinkRate => _blinkRate;
	public bool DestroyBigBubbleClick => _destroyBigBubbleClick;
	public bool BigBubbleSelfDestroy => _bigBubbleSelfDestroy;
	public float SelfDestroyTime => _selfDestroyTime;

	public Color ColorDummy => _colorDummy;
	public Color ColorA => _colorA;
	public Color ColorB => _colorB;
	public Color ColorC => _colorC;
	public Color ColorD => _colorD;
	public Color ColorE => _colorE;

	//public int PredatorFishesMaxCount => _predatorFishesMaxCount;

	//----------------------------------------------------------------

	[Header("Debug")]
	[SerializeField] private bool _godMode = false;

	[Header("Common")]
	[SerializeField] private BubbleColorMode _colorMode = BubbleColorMode.Implicit;
	[SerializeField] private int _livesCount = 3;
	//[SerializeField] private int _rescuedFishTargetCount = 6;

	//[Header("Tube behaviour")]
	//[SerializeField] private bool _TubeBubbleThrowDelay = false;
	//[SerializeField] private float _bubbleInitialBounceRate = -4;

	[Header("Bubble behaviour")]
	[SerializeField] private float _bubbleMoveSpeed = 8;
	[SerializeField] private float _bounceRate = 10;
	[SerializeField] private float _dragRate = 4;
	[SerializeField] private int _enlargeSizeClickCount = 2;

	[SerializeField] ClickEnlargeSizePair[] _clickEnlargeSizePairs;
	[SerializeField] private float _blinkRate = 0.15f;
	[SerializeField] private bool _destroyBigBubbleClick = false;
	[SerializeField] private bool _bigBubbleSelfDestroy = false;
	[SerializeField] private float _selfDestroyTime = 3;

	[Header("Fish color")]
	[SerializeField] private Color _colorDummy = Color.white;
	[SerializeField] private Color _colorA = Color.magenta;
	[SerializeField] private Color _colorB = Color.yellow;
	[SerializeField] private Color _colorC = Color.green;
	[SerializeField] private Color _colorD = Color.blue;
	[SerializeField] private Color _colorE = Color.cyan;

	//[Header("Predator fish")]
	//[SerializeField] private int _predatorFishesMaxCount;

	//----------------------------------------------------------------

	[Serializable]
	public struct ClickEnlargeSizePair
	{
		public int clickNUmber;
		public float sizeRate;
	}
}