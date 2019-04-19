using System;
using UnityEngine;

public enum Sounds
{
    bubbleClick,
    bubbleExplosion,
    feedFishGood,
	feedFishBad,
    fishDead,
	fishHappy,
	fishSpawn,
	backgroundMusic
}

[Serializable]
public struct Sound
{
    public Sounds name;
    public AudioClip audioClip;
    public float volume;
}
