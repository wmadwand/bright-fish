using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "NinjaEditor/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [SerializeField]
    List<Sound> data;

    public List<Sound> Data
    {
        get
        {
            return data;
        }
    }
}
