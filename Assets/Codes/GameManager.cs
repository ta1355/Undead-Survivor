using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    public static GameManager instance;
    public Player player;

    public PoolManager pool;

    void Awake()
    {
        instance = this;
    }

     void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime >maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }



}
