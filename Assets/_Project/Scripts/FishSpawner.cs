using System.Linq;
using UnityEngine;
using Zenject;

public class FishSpawner : MonoBehaviour
{
	public GameObject fish;

	public BubbleType[] coinTypeArray;

	public GameObject[] points;

	System.Random _rnd;
	Fish.FishDIFactory _fishDIFactory;

	[Inject]
	private void Construct(Fish.FishDIFactory fishDIFactory)
	{
		_fishDIFactory = fishDIFactory;
	}

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

	private void InitSpawn()
	{

		//int[] coinTypeArray = { 0, 1, 2 };
		BubbleType[] MyRandomArray = coinTypeArray.OrderBy(x => _rnd.Next()).ToArray();

		for (int i = 0; i < /*2*/ MyRandomArray.Length; i++)
		{
			var fish = _fishDIFactory.Create();
			fish.transform.SetPositionAndRotation(points[i].transform.position, Quaternion.identity);

			fish.Setup(MyRandomArray[i]);

		}
	}

	private void Spawn(BubbleType coinType, Vector3 position)
	{
		Fish fish = _fishDIFactory.Create();
		fish.transform.SetPositionAndRotation(position, Quaternion.identity);

		fish.Setup((BubbleType)Random.Range(0, coinTypeArray.Length)/*coinType*/);
	}
}
