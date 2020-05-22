using Assets.Scripts;
using UnityEngine;

[System.Serializable]
public class PlayerSetup : IPlayer
{
    [HideInInspector]
    public int Id;

    [HideInInspector]
    public float Speed;

    public string Name;
    public CharacterType CharacterType;
    public Sprite CharacterImage;
    public string ScriptName;

    string IPlayer.Name => Name;

    Sprite IPlayer.CharacterImage => CharacterImage;

    int IPlayer.Id => Id;
}
