using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public float DelayStartBy = 1f;
    public Rigidbody2D MyBody;
    public PlayerAnimations MyAnimator;
    public ExplodeAnimator MyExplosionAnimator;
    public GameObject MyUIPrefab;
    public PlayerAI MyAI;

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
    private bool canStart = false;
    private bool isDead = false;
    private bool hasWon = false;

    private PlayerUI myUI;
    private int stepsCounter = 0;

    void Start()
    {
        canStart = false;
        movementInputsBlocked = false;
        Invoke("InitializeUIControl", DelayStartBy - 1);
        Invoke("Initialize", DelayStartBy);
    }

    private void InitializeUIControl()
    {
        var uiObject = (Instantiate(MyUIPrefab, Vector2.zero, new Quaternion())) as GameObject;
        myUI = uiObject.GetComponent<PlayerUI>();
        myUI.Initialize(MyAI.MyName, 0, MyAnimator.GetMySprite, NextPlayerNumber);
    }

    private void Initialize()
    {
        canStart = true;
        UpdateCheckerStates();
    }

    private void Update()
    {
        if (!canStart) return;
        if (isDead) return;

        if (!movementInputsBlocked && !hasWon)
        {
            try
            {
                var selectedDirection = MyAI.RequestMove(GetPossibleDirections());
                if (!VerifySelection(selectedDirection))
                {
                    KillPlayer(selectedDirection);
                    return;
                }

                IncreaseSteps();
                if (selectedDirection == Direction.Down)
                {
                    Debug.Log("Tried to move Down");
                    nextTarget = new Vector2(MyBody.position.x, MyBody.position.y - 1);
                    movementInputsBlocked = true;
                    movement = new Vector2(0, -1);
                }
                if (selectedDirection == Direction.Up)
                {
                    Debug.Log("Tried to move Up");
                    nextTarget = new Vector2(MyBody.position.x, MyBody.position.y + 1);
                    movementInputsBlocked = true;
                    movement = new Vector2(0, 1);
                }
                if (selectedDirection == Direction.Left)
                {
                    Debug.Log("Tried to move Left");
                    nextTarget = new Vector2(MyBody.position.x - 1, MyBody.position.y);
                    movementInputsBlocked = true;
                    movement = new Vector2(-1, 0);
                }
                if (selectedDirection == Direction.Right)
                {
                    Debug.Log("Tried to move Right");
                    nextTarget = new Vector2(MyBody.position.x + 1, MyBody.position.y);
                    movementInputsBlocked = true;
                    movement = new Vector2(1, 0);
                }
            }
            catch
            {
                KillPlayer(true);
                return;
            }
        }

        MyAnimator.UpdateAnimation(movement);
    }

    private void FixedUpdate()
    {
        if (!canStart) return;
        if (isDead) return;

        if ((Vector2)MyBody.transform.position == nextTarget)
        {
            if (movement != Vector2.zero)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canStart) return;

        hasWon = true;
        myUI.OnFinalResults(1);
    }

    private void UpdateCheckerStates()
    {
        canGoLeft = !CheckLeft.IsTouchingWall;
        canGoRight = !CheckRight.IsTouchingWall;
        canGoUp = !CheckUp.IsTouchingWall;
        canGoDown = !CheckDown.IsTouchingWall;
    }

    private Direction[] GetPossibleDirections()
    {
        var directions = new List<Direction>();
        if (canGoDown) directions.Add(Direction.Down);
        if (canGoUp) directions.Add(Direction.Up);
        if (canGoRight) directions.Add(Direction.Right);
        if (canGoLeft) directions.Add(Direction.Left);

        return directions.ToArray();
    }

    private bool VerifySelection(Direction selection)
    {
        if (selection == Direction.Left && canGoLeft) return true;
        if (selection == Direction.Right && canGoRight) return true;
        if (selection == Direction.Up && canGoUp) return true;
        if (selection == Direction.Down && canGoDown) return true;

        return false;
    }

    private void KillPlayer(bool isException)
    {
        isDead = true;
        MyExplosionAnimator.TriggerExplosion();

        if (isException)
            myUI.OnError(ErrorType.Bug);

        Invoke("UpdateUIIcon", 0.5f);
    }

    private void UpdateUIIcon()
    {
        myUI.UpdateIcon(MyAnimator.GetMySprite);
    }

    private void KillPlayer(Direction direction)
    {
        KillPlayer(false);

        switch (direction)
        {
            case Direction.Down:
                myUI.OnError(ErrorType.Down);
                break;
            case Direction.Left:
                myUI.OnError(ErrorType.Left);
                break;
            case Direction.Right:
                myUI.OnError(ErrorType.Right);
                break;
            case Direction.Up:
                myUI.OnError(ErrorType.Up);
                break;
        }
    }

    private void IncreaseSteps()
    {
        stepsCounter++;
        myUI.UpdateSteps(stepsCounter);
    }

    private int NextPlayerNumber => GameObject.FindGameObjectsWithTag("Player").Length;
}