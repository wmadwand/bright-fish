using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSpawnProbability")]
public class FishSpawnProbability : ScriptableObject
{
	public List<FishSpawnWeightedItem> list = new List<FishSpawnWeightedItem>();
}

public enum FishCategory
{
	@default = 50,
	predator = 100,
	reversed = 150,
	rainbow = 200
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
