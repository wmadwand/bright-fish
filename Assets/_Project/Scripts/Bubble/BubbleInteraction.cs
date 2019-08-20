using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrightFish
{
	public class BubbleInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public event Action<float, bool, bool> OnInteract;

		[SerializeField] float _smoothing;

		private Vector2 _origin;
		private Vector2 _direction;
		private Vector2 _smoothDirection;
		private bool _touched;
		private int _pointerID;

		private Level _currentLevelSettings;
		private bool _isSwipeInProgress;

		//----------------------------------------------------------------		

		private void Awake()
		{
			_touched = false;
			_direction = Vector2.zero;

			_currentLevelSettings = _currentLevelSettings = GameController.Instance.levelController.CurrentLevel;
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
			//Debug.LogFormat("Swipe {0}, {1}", GetDirection().x, GetDirection().y);
			Debug.Log("released");

			// single click
			if (data.delta.normalized.y == 0)
			{
				if (GetComponent<BubbleView>().IsBubbleBurst || _isSwipeInProgress)
				{
					return;
				}

				if (data.pointerId == _pointerID)
				{
					_direction = Vector2.zero;
					_touched = false;

					transform.GetComponentInParent<Bubble>().OnClick();
					OnInteract(_currentLevelSettings.BubbleBounceUpSpeed, true, false);
				}
			}

			// or swipe
			else
			{
				if (_isSwipeInProgress)
				{
					return;
				}

				_isSwipeInProgress = true;

				OnInteract(_currentLevelSettings.BubbleSwipeSpeed * data.delta.normalized.y * -1, true, true);
			}
		}

		//private Vector2 GetDirection()
		//{
		//	_smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction, _smoothing * Time.deltaTime);
		//	return _smoothDirection;
		//}
	}
}