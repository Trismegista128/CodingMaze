using UnityEngine;
using TMPro;
using Assets.Scripts;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private float yPosition = 12.625f;
    [SerializeField]
    private Transform myPosition;
    [SerializeField]
    private TextMeshPro myName;
    [SerializeField]
    private TextMeshPro mySteps;
    [SerializeField]
    private SpriteRenderer myIcon;

    [SerializeField]
    private SpriteRenderer[] myMedals;
    [SerializeField]
    private SpriteRenderer[] myErrorIcons;

    public void Initialize(string name, int steps, Sprite characterIcon, int playerNumber)
    {
        myName.text = name;
        mySteps.text = CreateStepsString(steps);
        myIcon.sprite = characterIcon;
        myPosition.position = new Vector2(CalculatePostionXBaseOnNumber(playerNumber), yPosition);

        foreach(var medal in myMedals)
        {
            medal.enabled = false;
        }

        foreach(var error in myErrorIcons)
        {
            error.enabled = false;
        }
    }

    public void UpdateSteps(int value)
    {
        mySteps.text = CreateStepsString(value);
    }

    public void UpdateIcon(Sprite sprite)
    {
        myIcon.sprite = sprite;
    }

    public void OnFinalResults(int myPlace, int positionOnUI)
    {
        myPosition.position = new Vector2(CalculatePostionXBaseOnNumber(positionOnUI), yPosition);

        if (myPlace == 1) myMedals[0].enabled = true;
        if (myPlace == 2) myMedals[1].enabled = true;
        if (myPlace == 3) myMedals[2].enabled = true;
    }
    public void OnError(ErrorType error)
    {
        switch (error)
        {
            case ErrorType.Up:
                myErrorIcons[0].enabled = true;
                break;
            case ErrorType.Down:
                myErrorIcons[1].enabled = true;
                break;
            case ErrorType.Left:
                myErrorIcons[2].enabled = true;
                break;
            case ErrorType.Right:
                myErrorIcons[3].enabled = true;
                break;
            case ErrorType.Bug:
                myErrorIcons[4].enabled = true;
                break;
            case ErrorType.Looped:
                myErrorIcons[5].enabled = true;
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
