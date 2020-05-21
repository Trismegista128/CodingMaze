using UnityEngine;
using TMPro;
using Assets.Scripts;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private float YPosition = 12.625f;
    [SerializeField]
    private Transform MyPosition;
    [SerializeField]
    private TextMeshPro MyName;
    [SerializeField]
    private TextMeshPro MySteps;
    [SerializeField]
    private SpriteRenderer MyIcon;

    [SerializeField]
    private SpriteRenderer[] MyMedals;
    [SerializeField]
    private SpriteRenderer[] MyErrorIcons;

    public void Initialize(string name, int steps, Sprite characterIcon, int playerNumber)
    {
        MyName.text = name;
        MySteps.text = CreateStepsString(steps);
        MyIcon.sprite = characterIcon;
        MyPosition.position = new Vector2(CalculatePostionXBaseOnNumber(playerNumber), YPosition);

        foreach(var medal in MyMedals)
        {
            medal.enabled = false;
        }

        foreach(var error in MyErrorIcons)
        {
            error.enabled = false;
        }
    }

    public void UpdateSteps(int value)
    {
        MySteps.text = CreateStepsString(value);
    }

    public void UpdateIcon(Sprite sprite)
    {
        MyIcon.sprite = sprite;
    }

    public void OnFinalResults(int myPlace)
    {
        MyPosition.position = new Vector2(CalculatePostionXBaseOnNumber(myPlace), YPosition);

        if (myPlace == 1) MyMedals[0].enabled = true;
        if (myPlace == 2) MyMedals[1].enabled = true;
        if (myPlace == 3) MyMedals[2].enabled = true;
    }
    public void OnError(ErrorType error)
    {
        switch (error)
        {
            case ErrorType.Up:
                MyErrorIcons[0].enabled = true;
                break;
            case ErrorType.Down:
                MyErrorIcons[1].enabled = true;
                break;
            case ErrorType.Left:
                MyErrorIcons[2].enabled = true;
                break;
            case ErrorType.Right:
                MyErrorIcons[3].enabled = true;
                break;
            case ErrorType.Bug:
                MyErrorIcons[4].enabled = true;
                break;
        }
    }

    private string CreateStepsString(int steps)
    {
        if (steps < 10) return $"00{steps}";
        if (steps < 100) return $"0{steps}";
        return steps.ToString();
    }

    private int CalculatePostionXBaseOnNumber(int number)
    {
        return (number * 2) - 2;
    }
}
