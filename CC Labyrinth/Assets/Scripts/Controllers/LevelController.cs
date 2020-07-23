using UnityEngine;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public int LevelNumber;

    [SerializeField]
    private float uiYPosition = 12.625f;

    [HideInInspector]
    public bool HaveAllFinished => playersOnLevel.All(x => x.Value.HasFinished || x.Value.IsDead);

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

    public void ChangePlayersSpeed(float speed)
    {
        foreach(var player in playersOnLevel)
        {
            player.Value.ChangePlayerSpeed(speed);
        }
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
        playerController.InitializePlayer(player, Spawner.position, uiYPosition);

        playersOnLevel.Add(player.Id, playerController);

        yield return null;
    }

    public Dictionary<int, PlayerStats> GetLevelStats()
    {
        var listOfAll = new Dictionary<int, LevelStats>();

        //populate list
        foreach(var player in playersOnLevel)
        {
            var playerStat = player.Value.GatherStats();
            listOfAll.Add(player.Key, playerStat);
        }

        var successful = listOfAll.Where(x => x.Value.HasFinished);
        var loopFailures = listOfAll.Where(x => x.Value.ErrorType == ErrorType.Looped);
        var bugFailures = listOfAll.Where(x => x.Value.ErrorType == ErrorType.Bug);
        var restFailures = listOfAll.Where(x => 
            x.Value.HasFinished == false && 
            x.Value.ErrorType != ErrorType.Bug && 
            x.Value.ErrorType != ErrorType.Looped);

        var ordSuccess = successful.OrderBy(x => x.Value.StepsDone);
        var ordFailure = restFailures.OrderByDescending(x => x.Value.StepsDone);

        var distSuccessSteps = ordSuccess.Select(x => x.Value.StepsDone).Distinct().ToList();
        var distfailureSteps = ordFailure.Select(x => x.Value.StepsDone).Distinct().ToList();

        //First place those who finished level
        var lastPlacement = 0;
        for(var i = 0; i < distSuccessSteps.Count(); i++)
        {
            var value = distSuccessSteps[i];
            var playersWithScore = successful.Where(x => x.Value.StepsDone == value);
            foreach(var player in playersWithScore)
            {
                lastPlacement = i + 1;
                player.Value.Placement = lastPlacement;
            }
        }

        //Then place those who had to be killed as on the same place
        if (loopFailures.Any())
        {
            lastPlacement++;
            foreach (var loopBug in loopFailures)
            {
                loopBug.Value.Placement = lastPlacement;
            }
        }

        //Then those who entered the wall
        for (var i = 0; i < distfailureSteps.Count(); i++)
        {
            var value = distfailureSteps[i];
            var playersWithScore = restFailures.Where(x => x.Value.StepsDone == value);
            lastPlacement++;
            foreach (var player in playersWithScore)
            {
                player.Value.Placement = lastPlacement;
            }
        }

        //Last are those who provided buggy script.
        foreach (var bug in bugFailures)
        {
            bug.Value.Placement = lastPlacement + 1;
        }

        var levelStats = new Dictionary<int, PlayerStats>();
        foreach(var stat in listOfAll)
        {
            var playerStat = new PlayerStats();
            playerStat.Levels.Add(LevelNumber, stat.Value);
            levelStats.Add(stat.Key, playerStat);
        }

        return levelStats;
    }

    public void UpdateUIScores(Dictionary<int, PlayerStats> scores)
    {
        var positions = new Dictionary<int, int>();

        foreach(var score in scores)
        {
            var playerId = score.Key;
            var playerPosition = score.Value.Levels[LevelNumber].Placement;

            positions.Add(playerId, playerPosition);
        }

        var ordered = positions.OrderBy(x => x.Value).ToList();

        for(var i = 0; i < ordered.Count; i++)
        {
            playersOnLevel[ordered[i].Key].UpdateUIPosition(ordered[i].Value, i + 1);
        }
    }

    public void EmergencyKill()
    {
        foreach(var player in playersOnLevel)
        {
            if (player.Value.HasFinished || player.Value.IsDead) continue;

            player.Value.KillPlayer(ErrorType.Looped);
        }
    }
}
