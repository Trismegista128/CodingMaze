using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D MyBody;
    public PlayerAnimations MyAnimator;
    public float MySpeed = 1;

    public CheckerScript CheckLeft;
    public CheckerScript CheckRight;
    public CheckerScript CheckUp;
    public CheckerScript CheckDown;

    private bool movementInputsBlocked = false;
    private Vector2 nextTarget;
    private Vector2 movement = Vector2.zero;

    private bool canGoLeft;
    private bool canGoRight;
    private bool canGoUp;
    private bool canGoDown;

    void Start()
    {
        movementInputsBlocked = false;
        Invoke("Initialize", 1f);
    }

    private void Initialize()
    {
        UpdateCheckerStates();
    }

    void Update()
    {
        if (!movementInputsBlocked)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && canGoDown)
            {
                Debug.Log("Tried to move Down");
                nextTarget = new Vector2(MyBody.position.x, MyBody.position.y - 1);
                movementInputsBlocked = true;
                movement = new Vector2(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && canGoUp)
            {
                Debug.Log("Tried to move Up");
                nextTarget = new Vector2(MyBody.position.x, MyBody.position.y + 1);
                movementInputsBlocked = true;
                movement = new Vector2(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && canGoLeft)
            {
                Debug.Log("Tried to move Left");
                nextTarget = new Vector2(MyBody.position.x - 1, MyBody.position.y);
                movementInputsBlocked = true;
                movement = new Vector2(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && canGoRight)
            {
                Debug.Log("Tried to move Right");
                nextTarget = new Vector2(MyBody.position.x + 1, MyBody.position.y);
                movementInputsBlocked = true;
                movement = new Vector2(1, 0);
            }
        }

        MyAnimator.UpdateAnimation(movement);
    }

    private void FixedUpdate()
    {
        if ((Vector2)MyBody.transform.position == nextTarget)
        {
            if(movement != Vector2.zero)
                UpdateCheckerStates();
            movementInputsBlocked = false;
            movement = Vector2.zero;
        }

        //It's time to move
        if (movementInputsBlocked)
        {
            var move = Vector2.MoveTowards(MyBody.position, nextTarget, MySpeed * Time.deltaTime);
            MyBody.transform.position = move;
        }
    }

    private void UpdateCheckerStates()
    {
        canGoLeft = !CheckLeft.IsTouchingWall;
        canGoRight = !CheckRight.IsTouchingWall;
        canGoUp = !CheckUp.IsTouchingWall;
        canGoDown = !CheckDown.IsTouchingWall;
    }
}