using UnityEngine;
using Zenject;

public class MainMonoInstaller : MonoInstaller
{
	//[SerializeField] private GameSettings _gameSettings;

	public GameObject BubblePrefab;
	public GameObject BubbleFactoryPrefab;

	public override void InstallBindings()
	{
		var das = Container.Bind<GameSettings>().FromScriptableObjectResource("GameSettings").AsSingle().BindInfo;
		//Container.BindFactory<Bubble, Bubble.BFactory>();

		Container.BindFactory<Bubble, Bubble.BubbleDIFactory>().FromComponentInNewPrefab(BubblePrefab);
		Container.BindFactory<Tube, Tube.TubeDIFactory>().FromComponentInNewPrefab(BubbleFactoryPrefab);
	}
}