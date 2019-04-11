using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Coin>() && other.GetComponent<Coin>().IsReleased)
		{
			other.GetComponent<Coin>().SelfDestroy();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Coin>())
		{
			other.GetComponent<Coin>().SetReleased();
		}
	}
}

