using UnityEngine;

namespace BrightFish
{
	public class FishRotator : MonoBehaviour
	{
		public void Rotate()
		{
			transform.Rotate(0, 180, 0);
		}
	}
}