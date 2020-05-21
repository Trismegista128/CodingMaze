using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

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
    private bool isGamePlay = false;
    private bool isLevelInitialized = false;

    [HideInInspector]
    public bool IsReadyToContinue = false;

    [HideInInspector]
    public bool IsGamePlay { get { return isGamePlay; } }

    private void Start()
    {
        isGamePlay = false;
        isLevelInitialized = true;
        IsReadyToContinue = true;

        Statistics = new Dictionary<int, PlayerStats>();
        for(var i = 0; i < PlayersInGame.Length; i++)
        {
            var stat = new PlayerStats();
            Statistics.Add(i, stat);
        }

        UpdatePlayersData();

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        isLevelInitialized = false;
        IsReadyToContinue = false;

        if(level > 0)
        {
            isGamePlay = true;
            levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
            levelController.InitializePlayers(PlayersInGame, DelayBetweenPlayers);
            StartCoroutine(WaitForAllPlayersToSpawn());
        }
        else
        {
            isGamePlay = false;
            isLevelInitialized = true;
            IsReadyToContinue = true;
        }
    }

    void Update()
    {
        if (!isGamePlay) return;

        if (isLevelInitialized && levelController.HaveAllFinished && !IsReadyToContinue)
        {
            FinalizeLevel();
        }
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

    private IEnumerator WaitForAllPlayersToSpawn()
    {
        yield return new WaitForSeconds(PlayersInGame.Count() * DelayBetweenPlayers + 0.5f);
        isLevelInitialized = true;

        yield return null;
    }

    private void UpdateStats()
    {
        var currentLevelStats = levelController.GetLevelStats();

        foreach(var player in Statistics)
        {
            var id = player.Key;
            var stat = player.Value;

            var thisPlayerLevelStats = currentLevelStats[id].Levels.First();
            stat.Levels.Add(thisPlayerLevelStats.Key, thisPlayerLevelStats.Value);
        }

        levelController.UpdateUIScores(currentLevelStats);
    }

    public void FinalizeLevel()
    {
        UpdateStats();
        IsReadyToContinue = true;
    }

    public void LoadNextLevel()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void StartTheGame()
    {
        LoadNextLevel();
    }

    public void EmergencyKillAllPlayers()
    {
        levelController.EmergencyKill();
    }
}
