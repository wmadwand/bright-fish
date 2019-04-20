using System.Linq;
using UnityEngine;
using Zenject;

public class TubeSpawner : MonoBehaviour
{
	//[SerializeField] private readonly GameObject _tubePrefab;
	[SerializeField] private BubbleType[] _bubbleTypes;
	[SerializeField] private GameObject[] _tubeSpawnPoints;

	private System.Random _random;
	private Tube.TubeDIFactory _tubeDIFactory;

	//----------------------------------------------------------------

	[Inject]
	private void Construct(Tube.TubeDIFactory tubeDIFactory)
	{
		_tubeDIFactory = tubeDIFactory;
	}

	private void Awake()
	{
		_random = new System.Random();
	}

	private void Start()
	{
		SpawnTubes();
	}

	private void SpawnTubes()
	{
		//int[] coinTypeArray = { 0, 1, 2 };
		BubbleType[] randomArray = _bubbleTypes.OrderBy(x => _random.Next()).ToArray();

		for (int i = 0; i < /*2*/ randomArray.Length; i++)
		{
			Tube tube = _tubeDIFactory.Create();

			tube.transform.SetPositionAndRotation(_tubeSpawnPoints[i].transform.position, Quaternion.identity);
			tube.SetTubeID(i);
		}
	}
}