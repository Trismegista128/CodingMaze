using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts;

public class SummaryPlayerStats : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer myCharacterRenderer;

    [SerializeField]
    private TextMeshPro myTotalStepsText;

    [SerializeField]
    private TextMeshPro myNameText;

    [SerializeField]
    private Transform myTr;

    private GameController gameController;
    private GameObject[] resources;
    private Dictionary<int, SummaryLevelStats> levelStatsList;

    public void Initialize(PlayerStats stats, IPlayer playerData, int col)
    {
        resources = Resources.LoadAll<GameObject>("Summary");
        var levelStatsPrefab = resources.First(x => x.name == "LevelStats");

        myNameText.text = playerData.Name;
        myCharacterRenderer.sprite = playerData.CharacterImage;
        myTotalStepsText.text = stats.TotalSteps.ToString();

        levelStatsList = new Dictionary<int, SummaryLevelStats>();
        foreach(var level in stats.Levels)
        {
            var levelNumber = level.Key;
            var levelData = level.Value;

            var levelStatsObject = Instantiate(levelStatsPrefab, new Vector3(0, 0, 0), new Quaternion()) as GameObject;
            levelStatsObject.transform.parent = myTr;

            var levelStats = levelStatsObject.GetComponent<SummaryLevelStats>();
            levelStats.Initialize(levelNumber, levelData);

            levelStatsList.Add(levelNumber, levelStats);
        }
        myTr.position = new Vector3(col * 2, 0, 0);
    }
}
