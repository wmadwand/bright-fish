using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
	public GameSettings gameSettings;
	public Canvas canvas;

	public GameObject coinScoreTextPref;
}
