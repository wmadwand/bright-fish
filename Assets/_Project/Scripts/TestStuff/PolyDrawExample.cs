using UnityEngine;
using System.Collections;

public class PolyDrawExample : MonoBehaviour
{
	public int numberOfSides;
	public float polygonRadius;
	public Vector2 polygonCenter;

	void Update()
	{
		DebugDrawPolygon(polygonCenter, polygonRadius, numberOfSides);
	}

	// Draw a polygon in the XY plane with a specfied position, number of sides
	// and radius.
	void DebugDrawPolygon(Vector2 center, float radius, int numSides)
	{
		// The corner that is used to start the polygon (parallel to the X axis).
		Vector2 startCorner = new Vector2(radius, 0) + center;

		// The "previous" corner point, initialised to the starting corner.
		Vector2 previousCorner = startCorner;

		// For each corner after the starting corner...
		for (int i = 1; i < numSides; i++)
		{
			// Calculate the angle of the corner in radians.
			float cornerAngle = 2f * Mathf.PI / (float)numSides * i;

			// Get the X and Y coordinates of the corner point.
			Vector2 currentCorner = new Vector2(Mathf.Cos(cornerAngle) * radius, Mathf.Sin(cornerAngle) * radius) + center;

			// Draw a side of the polygon by connecting the current corner to the previous one.
			Debug.DrawLine(currentCorner, previousCorner);

			// Having used the current corner, it now becomes the previous corner.
			previousCorner = currentCorner;
		}

		// Draw the final side by connecting the last corner to the starting corner.
		Debug.DrawLine(startCorner, previousCorner);
	}
}