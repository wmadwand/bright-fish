using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TubeSpawner2D : MonoBehaviour
{

	public GameObject tubeOutPrefab;

	public BubbleType[] coinTypeArray;


	public GameObject[] tubeOutSpawnPoints;

	System.Random _rnd;

	private BubbleFactory.BubbleFactoryFactory _bubbleFactoryFactory;

	[Inject]
	void Construct(BubbleFactory.BubbleFactoryFactory bubbleFactoryFactory)
	{
		_bubbleFactoryFactory = bubbleFactoryFactory;
	}

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
		BubbleType[] MyRandomArray = coinTypeArray.OrderBy(x => _rnd.Next()).ToArray();

		for (int i = 0; i < /*2*/ MyRandomArray.Length; i++)
		{
			var tube = _bubbleFactoryFactory.Create();

			tube.transform.SetPositionAndRotation(tubeOutSpawnPoints[i].transform.position, Quaternion.identity);

			//var go = Instantiate(tubeOutPrefab, tubeOutSpawnPoints[i].transform.position, Quaternion.identity);
			//go.GetComponent<BubbleFactory>().Id = i;
			tube.Id = i;
		}
	}
}
