using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	[CreateAssetMenu(menuName = "FishSpawnProbability")]
	public class FishSpawnProbability : ScriptableObject
	{
		public List<FishSpawnWeightedItem> list = new List<FishSpawnWeightedItem>();
	}

	[Serializable]
	public struct FishSpawnWeightedItem
	{
		public FishCategory category;
		public int weight;
		public GameObject template;

		public FishSpawnWeightedItem(FishCategory category, int weight, GameObject template)
		{
			this.category = FishCategory.@default;
			this.weight = 100;
			this.template = null;
		}
	} 
}