using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightFish
{
	public class FishView : MonoBehaviour
	{
		public GameObject View => _view;

		[SerializeField] private GameObject _panicSign;
		[SerializeField] private GameObject _view;

		[SerializeField] private GameObject _particleTemplate;
		[SerializeField] private Transform _particleSpawnPoint;

		[SerializeField] private SpriteRenderer _bodySpriteRenderer;

		private Vector2 dropStartPos = new Vector2(0.35f, 0.6f);
		private Vector2 dropFinishPos = new Vector2(0.35f, 0.48f);

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

		public void ShowPaintSplash(Color color)
		{
			var obj = Instantiate(_particleTemplate, _particleSpawnPoint.position, Quaternion.identity);
			var script = obj.GetComponent<FishPaint>();

			script.SetColor(color);
		}

		public void UpdateHealthBar(float value)
		{
			//_bodySpriteRenderer.material.SetFloat("_Progress", value * .01f);
			_bodySpriteRenderer.material.SetFloat("_Fill", value * .01f);
		}

		//----------------------------------------------------------------

		private void Awake()
		{
			_panicSign.transform.localPosition = dropStartPos;
			_panicSign.SetActive(false);

			_sweatDropTween = _panicSign.transform.DOLocalMoveY(dropFinishPos.y, 1f).SetAutoKill(false);

			//_view.transform.DOShakePosition(1, .3f, 5, 20, false, false);
		}

		private void Update()
		{
			
		}
	}
}