using UnityEngine;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
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

    public Dictionary<int, LevelStats> GatherLevelStats()
    {
        var levelStats = new Dictionary<int, LevelStats>();

        foreach(var player in playersOnLevel)
        {
            var playerStats = player.Value.GatherStats();
            levelStats.Add(player.Key, playerStats);
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
