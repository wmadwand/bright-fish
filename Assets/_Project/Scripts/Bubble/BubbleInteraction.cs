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
				_direction = currentPosition - _origin;
			}
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData data)
		{
			Debug.Log($"data.scrollDelta = {data.scrollDelta}, IS data.dragging= {data.dragging}, data.delta.magnitude = {data.delta.magnitude}");

			// single click
			if (Mathf.Abs(_direction.y) < .5f)
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

				var normalizedDirY = _direction.normalized.y < 0 ? -1 : 1;
				OnInteract(_currentLevelSettings.BubbleSwipeSpeed * normalizedDirY * -1, true, true);

				GameController.Instance.sound.PlaySound(Sounds.bubbleSwipe);
			}
		}

		//private Vector2 GetDirection()
		//{
		//	_smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction, _smoothing * Time.deltaTime);
		//	return _smoothDirection;
		//}
	}
}