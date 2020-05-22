using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

public class SummaryController : MonoBehaviour
{
    private GameObject[] resources;
    private Dictionary<int, KeyValuePair<Transform, SummaryPlayerStats>> playerDataCollection;

    public void InitializeSummary(Dictionary<int, PlayerStats> stats, IPlayer[] players)
    {
        resources = Resources.LoadAll<GameObject>("Summary");
        var playerStatsPrefab = resources.First(x => x.name == "PlayerStats");

        playerDataCollection = new Dictionary<int, KeyValuePair<Transform, SummaryPlayerStats>>();
        foreach (var playerStat in stats)
        {
            var playerId = playerStat.Key;

            var playerStatsObject = Instantiate(playerStatsPrefab, new Vector3(0, 0, 0), new Quaternion()) as GameObject;
            var playerTransform = playerStatsObject.GetComponent<Transform>();
            var playerStats = playerStatsObject.GetComponent<SummaryPlayerStats>();

            playerDataCollection.Add(playerId, new KeyValuePair<Transform, SummaryPlayerStats>(playerTransform, playerStats));
        }

        var orderedStats = stats.OrderByDescending(x => x.Value.Levels.Sum(y => y.Value.HasFinished ? 1 : 0)).ThenBy(x => x.Value.TotalSteps).ToList();

        for (int col = 0; col < orderedStats.Count(); col++)
        {
            var playerId = orderedStats[col].Key;
            var playerStat = orderedStats[col].Value;
            var player = players.First(x => x.Id == playerId);

            //var summaryTransform = playerDataCollection[playerId].Key;
            var summaryStats = playerDataCollection[playerId].Value;

            summaryStats.Initialize(playerStat, player, col);
        }
    }
}
