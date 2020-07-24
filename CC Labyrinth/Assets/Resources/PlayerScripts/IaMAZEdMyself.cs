using UnityEngine;
using Assets.Scripts;
using Random = System.Random;
using System;
using System.Collections.Generic;

public class IaMAZEdMyself : MonoBehaviour, IPlayerAI
{

    //the following will be used in third version of the algorithm
    const int EARTH_RADIUS = 6371;
    const double EULERS_NUMBER = 2.71828;
    const double PI = 3.14159265359;
    const double GOLDEN_RATIO = 1.6180339887;
    const int LIGHT_SPEED = 299792458;
    const double STANDARD_GRAVITY = 9.8;
    const double EUR_TO_USD_EXCH_RATE = 1.16;
    const int SHELDON_COOPERS_FAVORITE_NUMBER = 73;
    const string EXCEPTION_MESSAGE = "https://www.wykop.pl/cdn/c3201142/comment_lyRi40o0Y0XJFWPW5OVtQxb5U12CEY1Z,w1200h627f.jpg";

    private string TeamName = "IAM";
    private CharacterType TeamCharacter = CharacterType.Joker;
    Random rnd = new Random();
    DirectionType previousMove = DirectionType.Up;

    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        try
        {
            if (possibleDirections.Length == 1)
            {
                var theOnlyWayToGo = possibleDirections[0];
                previousMove = DirectionOppositeTo(theOnlyWayToGo);
                return theOnlyWayToGo;
            }

            //never look back
            var newPossibilities = new List<DirectionType>(possibleDirections);
            newPossibilities.Remove(previousMove);

            var definitelyTheBestChoice = newPossibilities[rnd.Next(newPossibilities.Count)];
            previousMove = DirectionOppositeTo(definitelyTheBestChoice);

            return definitelyTheBestChoice;
        }
        catch (Exception) 
        {
            System.Diagnostics.Debug.WriteLine(EXCEPTION_MESSAGE);
        }

        //in case I screwed up the complex math, do it the old-fashioned way :D
        var thisIsHowIGoBackHomeFromABar = possibleDirections[rnd.Next(possibleDirections.Length)];
        previousMove = DirectionOppositeTo(thisIsHowIGoBackHomeFromABar);
        return thisIsHowIGoBackHomeFromABar;
    }

    private DirectionType DirectionOppositeTo(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Left:
                return DirectionType.Right;
            case DirectionType.Up:
                return DirectionType.Down;
            case DirectionType.Right:
                return DirectionType.Left;
            case DirectionType.Down:
                return DirectionType.Up;
            default:
                throw new NotImplementedException();
        }
    }

}

