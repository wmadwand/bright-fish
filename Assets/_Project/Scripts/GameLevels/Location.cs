using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "BrightFish/Location")]
public class Location : ScriptableObject
{
	public int ID => _id;
	public Level[] Levels => _levels;

	[SerializeField] private int _id;
	[SerializeField] private Level[] _levels;

}