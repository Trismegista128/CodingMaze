﻿using UnityEngine;
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

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void InitializeStats(int row, int col, LevelStats stats)
    {
        gameObject.SetActive(true);
        tr.position = new Vector2(CalculateXPos(col), CalculateYPos(row));

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
        if (row == 1) return 9.125f;
        if (row == 2) return 7.125f;
        if (row == 3) return 5.125f;
        if (row == 4) return 3.125f;

        return 1.125f;
    }

    private float CalculateXPos(int col)
    {
        return 4.125f;
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
