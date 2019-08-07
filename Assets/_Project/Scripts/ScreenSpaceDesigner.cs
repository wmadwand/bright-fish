using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public static class ScreenSpaceDesigner
	{
		private static float _step = .1f;
		private static float _centreHor = .5f;

		public static Vector2[] GetSpawnPoints(int count)
		{
			var result = new Vector2[count];

			switch (count)
			{
				case 1:
					{
						var screenPos = new Vector2(_centreHor, 0.92f);
						result[0] = Camera.main.ViewportToWorldPoint(screenPos);

						Debug.Log(1);
					}
					break;

				case 2:
					{
						var screenPos1 = new Vector2(0.4f, 0.95f);
						var screenPos2 = new Vector2(0.6f, 0.95f);

						result[0] = Camera.main.ViewportToWorldPoint(screenPos1);
						result[1] = Camera.main.ViewportToWorldPoint(screenPos2);

						Debug.Log(1);
					}
					break;

				case 3:
					{
						var screenPos1 = new Vector2(0.3f, 0.95f);
						var screenPos2 = new Vector2(0.5f, 0.95f);
						var screenPos3 = new Vector2(0.7f, 0.95f);

						result[0] = Camera.main.ViewportToWorldPoint(screenPos1);
						result[1] = Camera.main.ViewportToWorldPoint(screenPos2);
						result[2] = Camera.main.ViewportToWorldPoint(screenPos3);

						Debug.Log(1);
					}
					break;


				case 4:
					{
						var screenPos1 = new Vector2(0.2f, 0.95f);
						var screenPos2 = new Vector2(0.4f, 0.95f);
						var screenPos3 = new Vector2(0.6f, 0.95f);
						var screenPos4 = new Vector2(0.8f, 0.95f);


						result[0] = Camera.main.ViewportToWorldPoint(screenPos1);
						result[1] = Camera.main.ViewportToWorldPoint(screenPos2);
						result[2] = Camera.main.ViewportToWorldPoint(screenPos3);
						result[3] = Camera.main.ViewportToWorldPoint(screenPos4);

						Debug.Log(1);
					}
					break;

				case 5:
					{
						var screenPos1 = new Vector2(0.1f, 0.95f);
						var screenPos2 = new Vector2(0.3f, 0.95f);
						var screenPos3 = new Vector2(0.5f, 0.95f);
						var screenPos4 = new Vector2(0.7f, 0.95f);
						var screenPos5 = new Vector2(0.9f, 0.95f);


						result[0] = Camera.main.ViewportToWorldPoint(screenPos1);
						result[1] = Camera.main.ViewportToWorldPoint(screenPos2);
						result[2] = Camera.main.ViewportToWorldPoint(screenPos3);
						result[3] = Camera.main.ViewportToWorldPoint(screenPos4);
						result[4] = Camera.main.ViewportToWorldPoint(screenPos5);

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