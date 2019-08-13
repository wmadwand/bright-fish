using PathCreation;
using System;
using UnityEngine;

namespace BrightFish
{
	[CreateAssetMenu(fileName = "Level", menuName = "BrightFish/Level")]
	public class Level : ScriptableObject
	{
		public string ID => _id;
		public string Name => _name;
		public TubeSettings[] Tubes => _tubes;
		public FishCategory[] Fishes => _fishes;
		public int RescueFishTargetCount => _rescuedFishesTargetCount;
		public FishSpawnProbability FishSpawnProbability => _fishSpawnProbability;
		public int PredatorFishesMaxCount => _predatorFishesMaxCount;
		public ColorType[] ColorTypes => _colorTypes;

		public float BubbleMoveSpeed => _bubbleMoveSpeed;
		public float BounceRate => _bounceRate;
		public float BounceRateUp => _bounceRateUp;
		public float BounceRateDown => _bounceRateDown;
		public float DragRate => _dragRate;
		public float SpeedReflection => _speedReflection;
		public int EnlargeSizeClickCount => _enlargeSizeClickCount;

		[Header("Main")]
		[SerializeField] private string _id;
		[SerializeField] private string _name;
		[SerializeField] private Difficulty _difficulty; //TODO: incapsulate to separate class (fishesHealthStart, fishHealthReducingStepRate)
		[SerializeField] private float _timer;
		[SerializeField] private int _rescuedFishesTargetCount;
		[SerializeField] private ColorType[] _colorTypes;

		[Header("Tubes")]
		[SerializeField] private TubeSettings[] _tubes;

		[Header("Fishes")]
		[SerializeField] private FishCategory[] _fishes;
		[SerializeField] private FishSpawnProbability _fishSpawnProbability;
		[SerializeField] private int _predatorFishesMaxCount;
		[SerializeField] private bool _enablePredatorFishMovementAI;
		[SerializeField] private bool _enableFishMovementAI;
		[SerializeField] private float _fishHealthFadeRate;

		[Header("Bubble")]
		[SerializeField] private float _bubbleMoveSpeed = 8;
		[SerializeField] private float _bounceRate = 7;
		[SerializeField] private float _bounceRateUp = 7;
		[SerializeField] private float _bounceRateDown = 7;
		[SerializeField] private float _dragRate = 4;
		[SerializeField] private float _speedReflection = 5;
		[SerializeField] private int _enlargeSizeClickCount = 1;
	}

	[Serializable]
	public struct TubeSettings
	{
		public TubeType type;
		public float bubbleThrowDelay;
		public float bounceRateMin;
		public float bounceRateMax;
		public float bounceRateGrowthStep;
		public Difficulty _difficulty; //TODO: figure out the purpose

		public GameObject pathCreator;
	}
}