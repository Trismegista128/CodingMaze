using Assets.Scripts;
using UnityEngine;

[System.Serializable]
public class PlayerSetup
{
    [HideInInspector]
    public int Id;

    [HideInInspector]
    public float Speed;

    public string Name;
    public CharacterType CharacterType;
    public Sprite CharacterImage;
    public string ScriptName;
}
