using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
	public GameObject fish;

	public BubbleType[] coinTypeArray;

	public GameObject[] points;

	System.Random _rnd;

	private void Awake()
	{
		_rnd = new System.Random();

		Fish.OnDeath += Fish_OnDeath;
		Fish.OnHappy += Fish_OnHappy; ;
	}

	private void Fish_OnHappy(BubbleType arg1, Vector3 arg2)
	{
		Spawn(arg1, arg2);
	}

	private void Fish_OnDeath(BubbleType arg1, Vector3 arg2)
	{
		Spawn(arg1, arg2);
	}

	private void Start()
	{
		InitSpawn();
	}

	void InitSpawn()
	{

		//int[] coinTypeArray = { 0, 1, 2 };
		BubbleType[] MyRandomArray = coinTypeArray.OrderBy(x => _rnd.Next()).ToArray();

		for (int i = 0; i < /*2*/ MyRandomArray.Length; i++)
		{
			var tubeIn = Instantiate(fish, points[i].transform.position, Quaternion.identity);
			tubeIn.GetComponent<Fish>().Setup(MyRandomArray[i]);

		}
	}

	void Spawn(BubbleType coinType, Vector3 position)
	{
		var fishGo = Instantiate(fish, position, Quaternion.identity);
		fishGo.GetComponent<Fish>().Setup((BubbleType)Random.Range(0, coinTypeArray.Length)/*coinType*/);
	}
}
