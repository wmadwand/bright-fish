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

		public override void InstallBindings()
		{
			Container.Bind<GameSettings>().FromScriptableObjectResource("GameSettings").AsSingle();

			Container.BindFactory<Bubble, Bubble.BubbleDIFactory>().FromComponentInNewPrefab(_bubblePrefab);
			Container.BindFactory<Food, Food.FoodDIFactory>().FromComponentInNewPrefab(_foodPrefab);
			Container.BindFactory<Tube, Tube.TubeDIFactory>().FromComponentInNewPrefab(_tubePrefab);
			Container.BindFactory<Fish, Fish.FishDIFactory>().FromComponentInNewPrefab(_fishPrefab);
		}
	} 
}