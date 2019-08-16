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

		public float BubbleBaseSpeed => _bubbleBaseSpeed;		
		public float BubbleBounceUpRate => _bubbleBounceUpRate;
		public float BubbleBounceDownRate => _bubbleBounceDownRate;
		public float DragRate => _dragRate;
		public float BubbleBounceUpSpeed => _bubbleBounceUpSpeed;
		public int BubbleSwipeSpeed => _bubbleSwipeSpeed;
		public int BubbleEnlargeSizeClickCount => _bubbleEnlargeSizeClickCount;

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
		[SerializeField] private float _bubbleBaseSpeed = 2;
		[SerializeField] private float _bubbleBounceUpSpeed = 2;
		[SerializeField] private float _bubbleBounceUpRate = .6f;
		[SerializeField] private float _bubbleBounceDownRate = .95f;
		[HideInInspector][SerializeField] private float _dragRate = 4;

		[SerializeField] private int _bubbleSwipeSpeed = 5;
		[SerializeField] private int _bubbleEnlargeSizeClickCount = 1;
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