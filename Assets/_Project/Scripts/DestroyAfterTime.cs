using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	public float time;

	private void Awake()
	{
		GetComponent<ParticleSystem>().Stop();
		GetComponent<ParticleSystem>().Play();
	}


	void Start()
    {
		Destroy(gameObject, time);
    }


}
