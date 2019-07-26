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

		private Tween _sweatDropTween;

		//----------------------------------------------------------------

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
			_sweatDropTween.Play();			
		}

		public void OnPredatorLost()
		{
			_sweatDropTween.Rewind();

			_panicSign.SetActive(false);
			_panicSign.transform.localPosition = dropStartPos;
		}

		//----------------------------------------------------------------

		private void Awake()
		{
			_panicSign.transform.localPosition = dropStartPos;
			_panicSign.SetActive(false);

			_sweatDropTween = _panicSign.transform.DOLocalMoveY(dropFinishPos.y, 1f).SetAutoKill(false);
		}
	}
}