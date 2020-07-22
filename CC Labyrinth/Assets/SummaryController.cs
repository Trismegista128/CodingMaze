using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

public class SummaryController : MonoBehaviour
{
    private GameObject[] resources;
    private Dictionary<int, KeyValuePair<Transform, SummaryPlayerStats>> playerDataCollection;
    private List<KeyValuePair<int, PlayerStats>> orderedStats;

    private int PlayerStatsPerPage = 7;
    private int currentPage = 0;

    private void Update()
    {
        if (currentPage == 0) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPage == 1) return;

            currentPage--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if ((float)PlayerStatsPerPage >= (playerDataCollection.Count * 1.0f / currentPage)) return;
            currentPage++;
        }

        DisplayPage(currentPage);
    }

    public void InitializeSummary(Dictionary<int, PlayerStats> stats, IPlayer[] players, int page)
    {
        resources = Resources.LoadAll<GameObject>("Summary");
        var playerStatsPrefab = resources.First(x => x.name == "PlayerStats");

        if(playerDataCollection != null)
        {
            foreach(var data in playerDataCollection)
            {
                Destroy(data.Value.Key.gameObject);
            }
        }

        playerDataCollection = new Dictionary<int, KeyValuePair<Transform, SummaryPlayerStats>>();
        foreach (var playerStat in stats)
        {
            var playerId = playerStat.Key;

            var playerStatsObject = Instantiate(playerStatsPrefab, new Vector3(0, 0, 0), new Quaternion()) as GameObject;
            var playerTransform = playerStatsObject.GetComponent<Transform>();
            var playerStats = playerStatsObject.GetComponent<SummaryPlayerStats>();

            playerDataCollection.Add(playerId, new KeyValuePair<Transform, SummaryPlayerStats>(playerTransform, playerStats));
        }

        orderedStats = stats.OrderByDescending(x => x.Value.Levels.Sum(y => y.Value.FinishValue))
                                .ThenByDescending(x=>x.Value.Levels.Sum(y=>y.Value.ErrorValue))
                                .ThenBy(x => x.Value.TotalSteps).ToList();

        for (int col = 0; col < orderedStats.Count(); col++)
        {
            var playerId = orderedStats[col].Key;
            var playerStat = orderedStats[col].Value;
            var player = players.First(x => x.Id == playerId);

            //var summaryTransform = playerDataCollection[playerId].Key;
            var summaryStats = playerDataCollection[playerId].Value;

            summaryStats.Initialize(playerStat, player, col % PlayerStatsPerPage);
        }

        currentPage = page;
        DisplayPage(page);
    }

    public void DisplayPage(int page)
    {
        foreach(var stat in orderedStats)
        {
            var playerId = stat.Key;
            playerDataCollection[playerId].Value.gameObject.SetActive(false);
        }

        var skip = (page * PlayerStatsPerPage) - PlayerStatsPerPage;

        var paginatedList = orderedStats.Skip(skip).Take(PlayerStatsPerPage);

        foreach(var stat in paginatedList)
        {
            var playerId = stat.Key;
            playerDataCollection[playerId].Value.gameObject.SetActive(true);
        }
    }
}
