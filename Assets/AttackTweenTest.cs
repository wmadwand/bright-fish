using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTweenTest : MonoBehaviour
{
	public GameObject view;

	Sequence seq; 

	public void Play()
	{
		Animate();
	}

	private void Animate()
	{
		seq = DOTween.Sequence()
			.Append(view.transform.DOLocalMoveX(5, .2f))
			.AppendCallback(() => view.transform.localScale *= 2)
			.Append(view.transform.DOLocalMoveX(0, .2f))
			.AppendCallback(() => view.transform.localScale = Vector2.one);
	}
}
