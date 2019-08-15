using UnityEngine;
using UnityEngine.EventSystems;

namespace BrightFish
{
	public class BubbleMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		[SerializeField] float _smoothing;

		private Vector2 _origin;
		private Vector2 _direction;
		private Vector2 _smoothDirection;
		private bool _touched;
		private int _pointerID;

		private Level _currentLevelSettings;

		//----------------------------------------------------------------		

		private void Awake()
		{
			_touched = false;
			_direction = Vector2.zero;

			_currentLevelSettings = _currentLevelSettings = GameController.Instance.levelController.CurrentLevel;

			//GetComponent<Bubble>().OnBounce+=
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData data)
		{
			if (!_touched)
			{
				_touched = true;
				_pointerID = data.pointerId;
				_origin = data.position;
			}
		}

		void IDragHandler.OnDrag(PointerEventData data)
		{
			if (data.pointerId == _pointerID)
			{
				Vector2 currentPosition = data.position;
				Vector2 directionRaw = currentPosition - _origin;
				_direction = directionRaw.normalized;
			}
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData data)
		{
			Debug.LogFormat("Swipe {0}, {1}", GetDirection().x, GetDirection().y);
			Debug.Log("released");

			// single click
			if (data.delta.normalized.y == 0)
			{
				if (data.pointerId == _pointerID)
				{
					_direction = Vector2.zero;
					_touched = false;

					transform.GetComponentInParent<Bubble>().OnClick();
					AddForceDirection(/*GetDirection()*/);
				}
			}

			// or swipe
			else
			{
				if (GetComponent<Bubble>())
				{
					AddForceDirection(/*GetDirection(),*/ 10 * data.delta.normalized.y);
				}
				else
				{
					GetComponent<Food>().AddForceDirection(GetDirection());
				}
			}
		}

		private Vector2 GetDirection()
		{
			_smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction, _smoothing * Time.deltaTime);
			return _smoothDirection;
		}

		public void AddForceDirection(float direction = 1, bool isPlayerClick = true)
		{
			GetComponent<BubbleAlongPath>().AddBounceForce(_currentLevelSettings.SpeedReflection * direction, isPlayerClick);

			//_dir.Normalize();
			//_rigidbody2D.AddForce(_dir * _currentLevelSettings.SpeedReflection, ForceMode2D.Impulse);
		}
	}
}