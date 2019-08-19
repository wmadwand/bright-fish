using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
public class ParallaxEffect : MonoBehaviour
{
	[SerializeField] [Tooltip("From closest to furthest")] private List<Transform> backGround;
	[SerializeField] [Tooltip("From closest to furthest, closest objects should have biggest multipliers")] private List<float> speedMultipliers;
	[SerializeField] private float smoothing = 0.75f;
	[SerializeField] private Transform cameraToUse;
	private Vector3 previousCameraPosition;
	private Vector3 offset;
	void Start()
	{
		previousCameraPosition = cameraToUse.position;
	}
	void FixedUpdate()
	{
		for (int i = 0; i < backGround.Count; i++)
		{
			offset = (previousCameraPosition - cameraToUse.position) * speedMultipliers[i];
			backGround[i].position = Vector3.Lerp(backGround[i].position, backGround[i].position + offset, smoothing * Time.deltaTime);
		}
		previousCameraPosition = cameraToUse.position;
	}
}