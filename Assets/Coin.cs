using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnDestroy;

	public void OnPointerClick(PointerEventData eventData)
	{
		GetComponent<Rigidbody>().AddForce(Vector3.up * 1, ForceMode.Impulse);
	}
}
