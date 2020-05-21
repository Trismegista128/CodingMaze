using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("Players definition")]
    [SerializeField]
    private float DelayBetweenPlayers = 1f;
    [SerializeField]
    private float PlayersSpeed = 1f;

    [SerializeField]
    private PlayerSetup[] PlayersInGame;

    [Header("Player character prefabs")]
    public GameObject[] CharacterPrefabs;

    private Dictionary<int, PlayerStats> Statistics;
    private LevelController levelController;
    private void Start()
    {
        Statistics = new Dictionary<int, PlayerStats>();

        for(var i = 0; i < PlayersInGame.Length; i++)
        {
            var stat = new PlayerStats();
            Statistics.Add(i, stat);
        }

        UpdatePlayersData();

        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        levelController.InitializePlayers(PlayersInGame, DelayBetweenPlayers);
    }

    private void UpdatePlayersData()
    {
        for(var i = 0; i < PlayersInGame.Length; i++)
        {
            PlayersInGame[i].Id = i;
            PlayersInGame[i].Speed = PlayersSpeed;
        }
    }

    public GameObject GetCharacterPrefab(CharacterType charType)
    {
        switch (charType)
        {
            case CharacterType.Anna:
                return CharacterPrefabs[0];
            case CharacterType.Doctor:
                return CharacterPrefabs[1];
            case CharacterType.Renegate:
                return CharacterPrefabs[2];
            case CharacterType.Scout:
                return CharacterPrefabs[3];
            case CharacterType.Tapani:
                return CharacterPrefabs[4];
            case CharacterType.Vader:
                return CharacterPrefabs[5];
            case CharacterType.VanDame:
                return CharacterPrefabs[6];
            default:
                return null;
        }
    }

    private void UpdateStats()
    {
        var smth = levelController.GatherLevelStats();
    }
}
