using UnityEngine;

public class FishPaint : MonoBehaviour
{
	[SerializeField] private ParticleSystem[] _particleSystems;

	public void SetColor(Color value)
	{
		foreach (var ps in _particleSystems)
		{
			var main = ps.main;
			main.startColor = value;
		}
	}
}