using System;
using System.Collections.Generic;
using System.Linq;

public static class SRandom
{
	public static bool IsLuckyChanceWithPercent(int value, Random random)
	{
		return random.Next(1, 101) <= value ? true : false;
	}

	public static int GetWeightedRandomItemIndex(int[] array, Random random)
	{
		int pick = random.Next(array.Sum());
		int sum = 0;

		for (int i = 0; i < array.Length; i++)
		{
			sum += array[i];
			if (sum >= pick)
			{
				return i;
			}
		}

		return -1;
	}

	private static Random rng = new Random();

	// USAGE:
	// List<Product> products = GetProducts();
	// products.Shuffle();
	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static IList<T> Shuffle1<T>(this IList<T> list, Random random)
	{
		return list.OrderBy(x => random.Next()).ToArray();
	}
}