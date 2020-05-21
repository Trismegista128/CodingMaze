using UnityEngine;

public class NavigationController : MonoBehaviour
{
    public GameController GameController;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (GameController.IsGamePlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameController.EmergencyKillAllPlayers();
            }
        }

        if (!GameController.IsReadyToContinue) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.StartTheGame();
        }

    }
}
