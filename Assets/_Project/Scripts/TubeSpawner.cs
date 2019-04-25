using System;
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
	private Tube[] _currentTubeCollection;

	//----------------------------------------------------------------

	[Inject]
	private void Construct(Tube.TubeDIFactory tubeDIFactory)
	{
		_tubeDIFactory = tubeDIFactory;
	}

	private void Awake()
	{
		GameController.OnStart += GameController_OnStart;
		GameController.OnStop += GameController_OnStop;

		_random = new System.Random();
	}

	private void GameController_OnStop()
	{
		DestroyTubes();
	}

	private void GameController_OnStart()
	{
		SpawnTubes();
	}

	private void Start()
	{
		//SpawnTubes();
	}
	private void OnDestroy()
	{
		GameController.OnStart -= GameController_OnStart;
	}

	public void SpawnTubes()
	{
		_currentTubeCollection = new Tube[_bubbleTypes.Length];

		//int[] coinTypeArray = { 0, 1, 2 };
		BubbleType[] randomArray = _bubbleTypes.OrderBy(x => _random.Next()).ToArray();

		for (int i = 0; i < /*2*/ randomArray.Length; i++)
		{
			Tube tube = _tubeDIFactory.Create();

			tube.transform.SetPositionAndRotation(_tubeSpawnPoints[i].transform.position, Quaternion.identity);
			tube.SetTubeID(i);

			_currentTubeCollection[i] = tube;
		}
	}

	public void DestroyTubes()
	{
		Array.ForEach(_currentTubeCollection, item => Destroy(item.gameObject));
	}
}