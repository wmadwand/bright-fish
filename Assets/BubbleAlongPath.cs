using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleAlongPath : MonoBehaviour, IPointerUpHandler, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//OnClick();
	}

	void OnClick()
	{
		Debug.Log("click");
	}
}
