using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerSetup[] Players;
    private Dictionary<int, PlayerStats> Statistics;

    private LevelController levelController;
    private void Start()
    {
        Statistics = new Dictionary<int, PlayerStats>();

        for(var i = 0; i < Players.Length; i++)
        {
            var stat = new PlayerStats();
            Statistics.Add(i, stat);
        }

        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }
}
