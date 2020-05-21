using UnityEngine;
using Assets.Scripts;

public class CheckerScript : MonoBehaviour
{
    public CheckerType MyType;
    public bool IsTouchingWall;
    // Start is called before the first frame update

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
