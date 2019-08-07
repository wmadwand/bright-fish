using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public static class ScreenSpaceDesigner
	{
		public static Vector2[] GetSpawnPoints(int count)
		{
			var result = new Vector2[count];

			switch (count)
			{
				case 1:
					{

						var centre = Screen.width * 0.5f;

						//var screenPos = new Vector2(centre, Screen.height - 100);

						var screenPos = new Vector2(0.5f, 0.9f);

						//result[0] = Camera.main.ScreenToWorldPoint(screenPos);
						result[0] = Camera.main.ViewportToWorldPoint(screenPos);

						Debug.Log(1);
					}
					break;
			}

			return result;
		}

		static void DefineUsefulScreenSpace()
		{
			var scrWidthCentre = Screen.width * 0.5f;
		}

		private static bool IsPointVisibleCameraView(Vector3 point)
		{
			Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
			return (viewportPoint.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewportPoint));
		}
	}
}