using UnityEngine;
using Assets.Scripts;

public class CheckerScript : MonoBehaviour
{
    [SerializeField]
    private CheckerType myType;

    [HideInInspector]
    public bool IsTouchingWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsTouchingWall = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IsTouchingWall = true; 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsTouchingWall = false;
    }
}
