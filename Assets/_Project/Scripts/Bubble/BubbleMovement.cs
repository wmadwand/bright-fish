using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrightFish
{
	public class BubbleMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler
	{
		[SerializeField]
		float smoothing;

		private Vector2 origin;
		private Vector2 direction;
		private Vector2 smoothDirection;
		private bool touched;
		private int pointerID;

		private void Awake()
		{
			touched = false;
			direction = Vector2.zero;
		}

		public void OnPointerDown(PointerEventData data)
		{
			if (!touched)
			{
				touched = true;
				pointerID = data.pointerId;
				origin = data.position;
			}
		}

		public void OnDrag(PointerEventData data)
		{
			if (data.pointerId == pointerID)
			{
				Vector2 currentPosition = data.position;
				Vector2 directionRaw = currentPosition - origin;
				direction = directionRaw.normalized;
			}
		}

		public void OnPointerUp(PointerEventData data)
		{
			Debug.LogFormat("Swipe {0}, {1}", GetDirection().x, GetDirection().y);
			//OnSwipe(GetDirection());

			Debug.Log("released");

			if (GetComponent<Bubble>())
			{
				GetComponent<Bubble>().AddForceDirection(GetDirection());
			}
			else
			{
				GetComponent<Food>().AddForceDirection(GetDirection());
			}


			if (data.pointerId == pointerID)
			{
				direction = Vector2.zero;
				touched = false;

				transform.GetComponentInParent<Bubble>().OnClickThis();
			}
		}

		public Vector2 GetDirection()
		{
			smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing * Time.deltaTime);
			return smoothDirection;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			//transform.root.GetComponent<Bubble>().OnClick();
		}
	} 
}