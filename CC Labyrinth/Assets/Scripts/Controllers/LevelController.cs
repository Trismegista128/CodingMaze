using UnityEngine;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public int LevelNumber;

    [HideInInspector]
    public bool HaveAllFinished => playersOnLevel.All(x => x.Value.HasFinished);

    [SerializeField]
    private Transform Spawner;

    private Dictionary<int, PlayerController> playersOnLevel;

    private PlayerSetup[] playersData;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializePlayers(PlayerSetup[] players, float delay)
    {
        playersData = players;
        playersOnLevel = new Dictionary<int, PlayerController>();

        StartCoroutine(InitializePlayers(delay));
    }

    private IEnumerator InitializePlayers(float delay)
    {
        foreach(var player in playersData)
        {
            yield return StartCoroutine(InitializePlayer(player, delay));
        }
        yield return null;
    }

    private IEnumerator InitializePlayer(PlayerSetup player, float delay)
    {
        yield return new WaitForSeconds(delay);

        var playerPrefab = gameController.GetCharacterPrefab(player.CharacterType);
        var playerObject = Instantiate(playerPrefab, Spawner.position, new Quaternion()) as GameObject;
        var playerController = playerObject.GetComponent<PlayerController>();
        playerController.InitializePlayer(player, Spawner.position);

        playersOnLevel.Add(player.Id, playerController);

        yield return null;
    }

    public Dictionary<int, PlayerStats> GetLevelStats()
    {
        var listOfAllPlayersStats = new Dictionary<int, LevelStats>();

        //populate list
        foreach(var player in playersOnLevel)
        {
            var playerStat = player.Value.GatherStats();
            listOfAllPlayersStats.Add(player.Key, playerStat);
        }

        //order by place
        listOfAllPlayersStats.OrderBy(x => x.Value.StepsDone);

        //update players placement
        for(int i = 0; i < listOfAllPlayersStats.Count ; i++)
        {
            listOfAllPlayersStats[i].Placement = i + 1;
        }

        //sort again by player ID
        listOfAllPlayersStats.OrderBy(x => x.Key);

        var levelStats = new Dictionary<int, PlayerStats>();

        //generate collection of players and theirs level statistics
        foreach(var player in listOfAllPlayersStats)
        {
            var playerStat = new PlayerStats();
            playerStat.Levels.Add(LevelNumber, player.Value);
            levelStats.Add(player.Key, playerStat);
        }

        return levelStats;
    }

    public void TriggerDeaths()
    {
        foreach(var player in playersOnLevel)
        {
            player.Value.TriggerDeath();
        }
    }
}
