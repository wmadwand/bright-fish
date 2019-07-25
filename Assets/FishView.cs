using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public class FishView : MonoBehaviour
	{
		[SerializeField] private GameObject _panicSign;

		private Vector2 dropStartPos = new Vector2(0.76f, 0.5f);
		private Vector2 dropFinishPos = new Vector2(0.76f, 0.3f);

		Sequence _sequence;

		private void Awake()
		{
			_panicSign.transform.position = dropStartPos;
			_panicSign.SetActive(false);


		}

		public void OnPredatorFound()
		{
			//_sequence = DOTween.Sequence()
			//	   .Append(r.DOPunchScale(s, 0.25f))
			//	   .Join(g.DOFade(0, 0.15f).SetEase(Ease.InQuint))
			//	   .AppendCallback(() =>
			//	   {
			//		   g.SetActive(false);
			//		   _sequence = null;
			//	   });

			_panicSign.SetActive(true);
			//_panicSign.transform.DOMoveY(dropFinishPos.y, 1f);
		}

		public void OnPredatorLost()
		{
			_panicSign.transform.position = dropStartPos;
			_panicSign.SetActive(false);
		}
	}
}