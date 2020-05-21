using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    [Header("Player references")]
    [SerializeField]
    private Rigidbody2D myBody;
    [SerializeField]
    private CheckerScript checkLeft;
    [SerializeField]
    private CheckerScript checkRight;
    [SerializeField]
    private CheckerScript checkUp;
    [SerializeField]
    private CheckerScript checkDown;

    [HideInInspector]
    public Vector2 CurrentMovement = Vector2.zero;
    [HideInInspector]
    public int StepsCounter { get { return stepsCounter; } }

    private IPlayerAI myAI;
    private PlayerController playerController;
    private Vector2 nextTarget;

    private float mySpeed;

    private bool movementInputsBlocked = false;
    private bool canGoLeft;
    private bool canGoRight;
    private bool canGoUp;
    private bool canGoDown;
    private bool canStart = false;

    private int stepsCounter = 0;

    public void Initialize(PlayerController controller, IPlayerAI aiscript, float delay, float speed)
    {
        playerController = controller;
        myAI = aiscript;
        mySpeed = speed;
        Invoke("InitializeDelayed", delay);
    }

    private void InitializeDelayed()
    {
        canStart = true;
        UpdateCheckerStates();
    }

    private void Update()
    {
        if (!canStart) return;
        if (playerController.IsDead) return;

        if (!movementInputsBlocked && !playerController.HasFinished)
        {
            try
            {
                var selectedDirection = myAI.RequestMove(GetPossibleDirections());
                if (!VerifySelection(selectedDirection))
                {
                    KillPlayer(selectedDirection);
                    return;
                }

                stepsCounter++;
                if (selectedDirection == DirectionType.Down)
                {
                    nextTarget = new Vector2(myBody.position.x, myBody.position.y - 1);
                    movementInputsBlocked = true;
                    CurrentMovement = new Vector2(0, -1);
                }
                if (selectedDirection == DirectionType.Up)
                {
                    nextTarget = new Vector2(myBody.position.x, myBody.position.y + 1);
                    movementInputsBlocked = true;
                    CurrentMovement = new Vector2(0, 1);
                }
                if (selectedDirection == DirectionType.Left)
                {
                    nextTarget = new Vector2(myBody.position.x - 1, myBody.position.y);
                    movementInputsBlocked = true;
                    CurrentMovement = new Vector2(-1, 0);
                }
                if (selectedDirection == DirectionType.Right)
                {
                    nextTarget = new Vector2(myBody.position.x + 1, myBody.position.y);
                    movementInputsBlocked = true;
                    CurrentMovement = new Vector2(1, 0);
                }
            }
            catch
            {
                KillPlayer(true);
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!canStart) return;
        if (playerController.IsDead) return;

        if ((Vector2)myBody.transform.position == nextTarget)
        {
            if (CurrentMovement != Vector2.zero)
                UpdateCheckerStates();
            movementInputsBlocked = false;
            CurrentMovement = Vector2.zero;
        }

        //It's time to move
        if (movementInputsBlocked)
        {
            var move = Vector2.MoveTowards(myBody.position, nextTarget, mySpeed * Time.deltaTime);
            myBody.transform.position = move;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canStart) return;

        var myLayer = gameObject.layer;
        var coliderLayer = collision.gameObject.layer;
        var ignore = Physics2D.GetIgnoreLayerCollision(myLayer, coliderLayer);
        if (ignore) return;

        playerController.TriggerWin();
    }

    private void UpdateCheckerStates()
    {
        canGoLeft = !checkLeft.IsTouchingWall;
        canGoRight = !checkRight.IsTouchingWall;
        canGoUp = !checkUp.IsTouchingWall;
        canGoDown = !checkDown.IsTouchingWall;
    }

    private DirectionType[] GetPossibleDirections()
    {
        var directions = new List<DirectionType>();
        if (canGoDown) directions.Add(DirectionType.Down);
        if (canGoUp) directions.Add(DirectionType.Up);
        if (canGoRight) directions.Add(DirectionType.Right);
        if (canGoLeft) directions.Add(DirectionType.Left);

        return directions.ToArray();
    }

    private bool VerifySelection(DirectionType selection)
    {
        if (selection == DirectionType.Left && canGoLeft) return true;
        if (selection == DirectionType.Right && canGoRight) return true;
        if (selection == DirectionType.Up && canGoUp) return true;
        if (selection == DirectionType.Down && canGoDown) return true;

        return false;
    }

    private void KillPlayer(bool isException)
    {
        playerController.KillPlayer(isException ? ErrorType.Bug : ErrorType.None);
    }

    private void KillPlayer(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Down:
                playerController.KillPlayer(ErrorType.Down);
                break;
            case DirectionType.Left:
                playerController.KillPlayer(ErrorType.Left);
                break;
            case DirectionType.Right:
                playerController.KillPlayer(ErrorType.Right);
                break;
            case DirectionType.Up:
                playerController.KillPlayer(ErrorType.Up);
                break;
        }
    }
}