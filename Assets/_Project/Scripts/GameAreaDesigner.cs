using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public static class GameAreaDesigner
	{
		private static float _offset = .2f;
		private static float _maxCount = 5;
		private static float _xMin = 0.1f;
		private static float _yPosTop = 0.95f;
		private static float _yPosBottom = 0.1f;

		public static Vector2[] GetSpawnPoints(int count, SpawnPointPosition position)
		{
			var result = new Vector2[count];
			var rate = Mathf.Abs(count - _maxCount) * 0.1f;
			var yPos = position == SpawnPointPosition.Top ? _yPosTop : _yPosBottom;
			var _xMin = 0.1f;

			for (int i = 0; i < count; i++)
			{
				var vec = new Vector2(_xMin + rate, yPos);
				result[i] = Camera.main.ViewportToWorldPoint(vec);
				_xMin += _offset;
			}

			return result;
		}		

		private static bool IsPointVisibleCameraView(Vector3 point)
		{
			Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
			return (viewportPoint.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewportPoint));
		}
	}
}