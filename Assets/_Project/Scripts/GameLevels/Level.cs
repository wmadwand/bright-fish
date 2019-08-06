using System;
using UnityEngine;

namespace BrightFish
{
	[CreateAssetMenu(fileName = "Level", menuName = "BrightFish/Level")]
	public class Level : ScriptableObject
	{
		public string ID => _id;

		public TubeItem[] Tubes => _tubes;

		[SerializeField] private string _id;
		[SerializeField] private Difficulty _difficulty; //TODO: incapsulate to separate class (fishesHealthStart, fishHealthReducingStepRate)
		[SerializeField] private float _timer;
		[SerializeField] private TubeItem[] _tubes;
		[SerializeField] private FishCategory[] _fishes;
		[SerializeField] private int _rescuedFishesTargetCount;
		[SerializeField] private FishSpawnProbability _fishSpawnProbability;
		[SerializeField] private int _predatorFishesMaxCount;
		[SerializeField] private bool _enablePredatorFishMovementAI;
		[SerializeField] private bool _enableFishMovementAI;
		[SerializeField] private float _fishHealthFadeRate;
	}

	[Serializable]
	public struct TubeItem
	{
		public TubeType tubeType;
		public float bounceRateMin;
		public float bounceRateMax;
		public float bounceRateGrowthStep;
		public Difficulty _difficulty; //TODO: figure out the purpose
	}
}