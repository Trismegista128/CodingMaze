using UnityEngine;

namespace Assets.Scripts
{
    public interface IPlayer
    {
        int Id { get; }
        string Name { get; }
        Sprite CharacterImage { get; }
    }
}
