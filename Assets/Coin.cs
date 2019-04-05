using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnDestroy;
	public float bounceRate;

	public void OnPointerClick(PointerEventData eventData)
	{
		GetComponent<Rigidbody>().AddForce(Vector3.up * bounceRate, ForceMode.Impulse);

		Debug.Log("click");
	}

	void Enlarge()
	{

	}

}
