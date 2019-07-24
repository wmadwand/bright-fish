using UnityEngine;
using Zenject;

namespace BrightFish
{
	public class MainMonoInstaller : MonoInstaller
	{
		[SerializeField] GameObject _bubblePrefab;
		[SerializeField] GameObject _foodPrefab;
		[SerializeField] GameObject _tubePrefab;
		[SerializeField] GameObject _fishPrefab;
		[SerializeField] GameObject _fishPredatorPrefab;

		public override void InstallBindings()
		{
			Container.Bind<GameSettings>().FromScriptableObjectResource("GameSettings").AsSingle();
			Container.Bind<GameSettings>().FromScriptableObjectResource("FishSpawnProbability").AsSingle();

			Container.BindFactory<Bubble, Bubble.BubbleDIFactory>().FromComponentInNewPrefab(_bubblePrefab).UnderTransformGroup("Bubbles");
			Container.BindFactory<Food, Food.FoodDIFactory>().FromComponentInNewPrefab(_foodPrefab).UnderTransformGroup("Foods");
			Container.BindFactory<Tube, Tube.TubeDIFactory>().FromComponentInNewPrefab(_tubePrefab).UnderTransformGroup("Tubes");

			Container.BindFactory<Fish, Fish.FishDIFactory>().FromComponentInNewPrefab(_fishPrefab).UnderTransformGroup("Fishes");
			Container.BindFactory<Fish, Fish.FishPredatorDIFactory>().FromComponentInNewPrefab(_fishPredatorPrefab).UnderTransformGroup("Fishes");
		}
	} 
}