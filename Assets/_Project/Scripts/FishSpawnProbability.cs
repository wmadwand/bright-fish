using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	[CreateAssetMenu(menuName = "FishSpawnProbability")]
	public class FishSpawnProbability : ScriptableObject
	{
		public List<FishSpawnWeightedItem> list = new List<FishSpawnWeightedItem>();

		public int[] GetWeightsArray()
		{
			int[] arr = new int[list.Count];

			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = list[i].weight;
			}

			return arr;
		}
	}

	[Serializable]
	public struct FishSpawnWeightedItem
	{
		public FishCategory category;
		public int weight;
		public GameObject template;

		public FishSpawnWeightedItem(FishCategory category, int weight, GameObject template)
		{
			this.category = FishCategory.peaceful;
			this.weight = 100;
			this.template = null;
		}
	}
}