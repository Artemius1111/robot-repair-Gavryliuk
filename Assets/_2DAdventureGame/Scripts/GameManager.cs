using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public UIHandler uiHandler;

    EnemyController[] enemies;
    bool gameOver = false;

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (player.health <= 0)
        {
            gameOver = true;
            uiHandler.DisplayLoseScreen();
            Invoke(nameof(ReloadScene), 3f);
        }
        else if (AllEnemiesFixed())
        {
            gameOver = true;
            uiHandler.DisplayWinScreen();
            Invoke(nameof(ReloadScene), 3f);
        }
    }

    bool AllEnemiesFixed()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.isBroken)
            {
                return false;
            }
        }
        return true;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
