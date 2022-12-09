using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviourSingleton<GameSceneManager>
{
    [SerializeField] private MenuCtrl menuCtrl;
    [SerializeField] private EnemysCtrl enemys;
    [SerializeField] private PlayerCtrl player;
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerHealthUI playerHealthUI;
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private Transform playerSpawnPoint;

    [SerializeField] private int maxLevel = 5;
    public float levelStartTime { get; private set; }
    private int _currentLevel;
    public int currentLevel
    {
        get { return _currentLevel; }
        set
        {
            _currentLevel = value;
            levelUI.RefreshUI(_currentLevel);
        }
    }

    public bool CanStart { get; set; }
    public bool GameStart { get; private set; }
    private bool gamePause;

    private void Update()
    {
        if(GameStart && Input.GetButtonDown("Submit"))
        {
            GamePause();
        }
    }

    public void StartGame()
    {
        if (!CanStart) return;
        StartCoroutine(DoStartGame());
    }

    private IEnumerator DoStartGame()
    {
        menuCtrl.GameStart();
        playerHealthUI.UpdateUI(playerHealth.MaxHP);
        currentLevel = 1;
        enemys.Initialization();
        yield return new WaitForSeconds(2);
        PreparePlayer();
        playerScore.Initialization();
        playerHealth.Initialization();
        yield return new WaitForSeconds(1);
        GameStart = true;
    }

    private void GamePause()
    {
        gamePause = !gamePause;
        menuCtrl.SetPauseUI(gamePause);
        Time.timeScale = gamePause ? 0 : 1;
    }

    public void GameOver()
    {
        enemys.Stop();
        enemys.HideEnemys();
        menuCtrl.GameOverMove();
        GameStart = false;
    }

    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            player.gameObject.SetActive(false);
            GameOver();
        }
        else
        {
            levelStartTime = Time.time;
            enemys.Initialization();
        }
    }

    public void RebornPlayer()
    {
        StartCoroutine(DoRebornPlayer());
    }

    private IEnumerator DoRebornPlayer()
    {
        enemys.StopAttack();
        yield return new WaitUntil(() => enemys.currentFlyingEnemyCount == 0);
        PreparePlayer();
        yield return new WaitForSeconds(1);
        enemys.StartAttack();
    }
    private void PreparePlayer()
    {
        StartCoroutine(DoPreparePlayer());
    }
    private IEnumerator DoPreparePlayer()
    {
        menuCtrl.SetPrepareUI(true);
        yield return new WaitForSeconds(1);
        menuCtrl.SetPrepareUI(false);
        player.transform.position = playerSpawnPoint.position;
        player.gameObject.SetActive(true);
        enemys.StartAttack();
    }

    public void Quit()
    {
        Application.Quit();
    }
    private void OnApplicationQuit()
    {
        playerScore.SaveBestScore();
    }
}
