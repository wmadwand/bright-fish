using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public class BubbleView : MonoBehaviour
	{
		private GameObject _view;

		private void Awake()
		{
			_view = GetComponentInChildren<Renderer>().gameObject;
		}

		public void ShakeBubble(bool isClick = true)
		{
			if (isClick)
			{
				_view.transform.DOShakeScale(.4f, .2f, 10, 45);

			}
			else
			{
				_view.transform.DOShakeScale(.2f, .1f, 1, 5);
			}
		}
	}
}