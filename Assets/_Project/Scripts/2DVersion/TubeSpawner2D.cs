using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TubeSpawner2D : MonoBehaviour
{
	
	public GameObject tubeOutPrefab;

	public CoinType[] coinTypeArray;

	
	public GameObject[] tubeOutSpawnPoints;

	System.Random _rnd;

	private void Awake()
	{
		_rnd = new System.Random();
	}

	private void Start()
	{
		GenerateTubes();
	}

	void GenerateTubes()
	{

		//int[] coinTypeArray = { 0, 1, 2 };
		CoinType[] MyRandomArray = coinTypeArray.OrderBy(x => _rnd.Next()).ToArray();

		for (int i = 0; i < /*2*/ MyRandomArray.Length; i++)
		{
			var go = Instantiate(tubeOutPrefab, tubeOutSpawnPoints[i].transform.position, Quaternion.identity);
			go.GetComponent<BubbleFactory>().Id = i;
		}
	}
}
