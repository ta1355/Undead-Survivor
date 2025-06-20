using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int playerId;
    public float health;

    public float maxHealth = 100;
    public int level;

    public int kill;

    public int exp;

    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public Player player;

    public PoolManager pool;

    public LevelUp uiLevelUp;

    public Result uiResult;

    public GameObject enemyCleaner;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void GameStart(int id)
    {

        playerId = id;

        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.select(playerId % 2);
        Resume();

        AudioManager.instance.PlayBgm(true); // 배경음악 시작
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false); // 배경음악 정지
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

     public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;

        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false); // 배경음악 정지
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    void Update()
    {
        if (isLive == false)
        {
            return;
        }
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {

        if (isLive == false)
        {
            return;
        }

        exp++;

        if (exp == nextExp[Mathf.Min(level,nextExp.Length -1)])
        {
            level++;
            exp = 0;
            uiLevelUp.show();
        }
    }


    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;    
    }
}
