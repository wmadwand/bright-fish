using PathCreation;
using UnityEngine;

namespace BrightFish
{
	public class BubblePathFollower : MonoBehaviour
	{
		public PathCreator pathCreator;
		public EndOfPathInstruction endOfPathInstruction;
		public float speed = 5;
		float distanceTravelled;

		void Start()
		{
			if (pathCreator != null)
			{
				pathCreator.pathUpdated += OnPathChanged;
			}
		}

		void FixedUpdate()
		{
			if (pathCreator != null)
			{
				distanceTravelled += speed * Time.fixedDeltaTime;
				transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

				//transform.GetComponent<Rigidbody2D>().position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

				//transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
			}
		}

		// If the path changes during the game, update the distance travelled so that the follower's position on the new path
		// is as close as possible to its position on the old path
		void OnPathChanged()
		{
			distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
		}
	}
}