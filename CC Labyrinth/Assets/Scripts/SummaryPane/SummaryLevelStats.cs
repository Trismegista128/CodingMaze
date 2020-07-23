using UnityEngine;
using TMPro;
using Assets.Scripts;

public class SummaryLevelStats : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro steps;
    [SerializeField]
    private SpriteRenderer cross;
    [SerializeField]
    private SpriteRenderer checkMark;
    [SerializeField]
    private Transform tr;

    [SerializeField]
    private Color GoldColor;
    [SerializeField]
    private Color SilverColor;
    [SerializeField]
    private Color BronzeColor;
    [SerializeField]
    private Color GreenColor;
    [SerializeField]
    private Color RedColor;

    public void Initialize(int row, LevelStats stats)
    {
        tr.position = new Vector2(4.125f, CalculateYPos(row));

        var color = stats.HasFinished ? SelectColor(stats) : RedColor;
        if (stats.HasFinished)
        {
            cross.enabled = false;
            checkMark.enabled = true;
            checkMark.color = color;
        }
        else
        {
            checkMark.enabled = false;
            cross.enabled = true;
            cross.color = color;
        }

        steps.text = stats.StepsDone.ToString();
    }

    private float CalculateYPos(int row)
    {
        return 9.125f - row * 2;
        //if (row == 0) return 9.125f;
        //if (row == 1) return 7.125f;
        //if (row == 2) return 5.125f;
        //if (row == 3) return 3.125f;
        //if (row == 4) return 1.125f;
        //if (row == 5) return 
        //return 1.125f;
    }

    private Color SelectColor(LevelStats stats)
    {
        var placement = stats.Placement;
        if (placement == 1) return GoldColor;
        if (placement == 2) return SilverColor;
        if (placement == 3) return BronzeColor;

        return GreenColor;
    }
}
