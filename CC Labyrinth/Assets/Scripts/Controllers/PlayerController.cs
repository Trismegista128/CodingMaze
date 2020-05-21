using UnityEngine;
using Assets.Scripts;

public class PlayerController : MonoBehaviour
{
    public float DelayMovementForSeconds = 2f;

    [Header("Player references")]
    [SerializeField]
    private PlayerAnimations playerAnimationController;

    [SerializeField]
    private Movement playerMovementController;

    [SerializeField]
    private GameObject playerUIPrefab;
    private PlayerUI myUI;

    [HideInInspector]
    public bool IsDead { get { return imDead; } }
    [HideInInspector]
    public bool HasFinished { get { return iWon; } }

    private string myName;
    private int mySteps;
    private int myId;

    private bool imDead;
    private bool iWon;
    private bool isInitialized = false;
    private ErrorType error;

    private void Update()
    {
        if (!isInitialized) return;

        playerAnimationController.UpdateAnimation(playerMovementController.CurrentMovement);

        if(myUI != null)
            myUI.UpdateSteps(playerMovementController.StepsCounter);
    }

    public void InitializePlayer(PlayerSetup player, Vector2 startingPostion)
    {
        myId = player.Id;
        myName = player.Name;

        mySteps = 0;
        imDead = false;
        iWon = false;

        Invoke(nameof(InitializeUIControl), DelayMovementForSeconds-1);

        var asset = Resources.Load<Object>(player.ScriptName) as GameObject;
        var script = asset.GetComponent<IPlayerAI>();

        playerMovementController.Initialize(this, script, DelayMovementForSeconds, player.Speed);
        playerAnimationController.SetSortingOrder(player.Id);

        isInitialized = true;
    }

    private void InitializeUIControl()
    {
        var uiObject = (Instantiate(playerUIPrefab, Vector2.zero, new Quaternion())) as GameObject;
        myUI = uiObject.GetComponent<PlayerUI>();
        myUI.Initialize(myName, mySteps, playerAnimationController.GetMySprite, myId + 1);
    }

    public LevelStats GatherStats()
    {
        var stats = new LevelStats();

        //take infor from somewhere;
        stats.HasFinished = iWon;
        stats.StepsDone = playerMovementController.StepsCounter;
        stats.ErrorType = error;

        return stats;
    }

    public void KillPlayer(ErrorType error)
    {
        imDead = true;
        this.error = error;
        Invoke(nameof(UpdateUIImage), 0.5f);
        playerAnimationController.TriggerExplosion();
    }

    public void TriggerDeath()
    {

    }

    public void TriggerWin()
    {
        iWon = true;
    }

    private void UpdateUIImage()
    {
        if(myUI != null)
            myUI.UpdateIcon(playerAnimationController.GetMySprite);
    }
}
