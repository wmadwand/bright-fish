using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorMode
{
	Explicit,
	Implicit
}

[CreateAssetMenu(fileName ="GameSettings")]
public class GameSettings : ScriptableObject
{
	public ColorMode colorMode;

}
