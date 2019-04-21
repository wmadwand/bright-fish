using UnityEngine;
using Zenject;

public class MainMonoInstaller : MonoInstaller
{
	//[SerializeField] private GameSettings _gameSettings;

	public GameObject BubblePrefab;
	public GameObject TubePrefab;
	public GameObject FishPrefab;

	public override void InstallBindings()
	{
		Container.Bind<GameSettings>().FromScriptableObjectResource("GameSettings").AsSingle();

		Container.BindFactory<Bubble, Bubble.BubbleDIFactory>().FromComponentInNewPrefab(BubblePrefab);
		Container.BindFactory<Tube, Tube.TubeDIFactory>().FromComponentInNewPrefab(TubePrefab);
		Container.BindFactory<Fish, Fish.FishDIFactory>().FromComponentInNewPrefab(FishPrefab);
	}
}