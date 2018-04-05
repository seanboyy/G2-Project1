using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Constants : MonoBehaviour {

    // Singleton stuff
    public static Constants instance;
    [Header("Use for testing")]
    public bool testMode = false;

    public Image image;
    private int playerLives = 5;
    public Vector3 playerPos;
    public Text scoreText;

    [SerializeField]
    private int score = 0;

    [SerializeField]
    private int numEnemies = 0;

    [SerializeField]
    private int wavesCleared = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        Messenger.AddListener(Messages.ENEMY_SPAWNED, EnemySpawned);
        Messenger<Enemy>.AddListener(Messages.ENEMY_DESTROYED, EnemyShipDestroyed);
        Messenger.AddListener(Messages.PLAYER_DESTROYED, PlayerShipDestroyed);
        Messenger.AddListener(Messages.WAVE_CLEAR, WavesClearedCounter);
    }

	// Use this for initialization
	void Start () {
        if (!testMode)
            image.rectTransform.sizeDelta = new Vector2(25 * (playerLives - 1), 33);
	}
	
	// Update is called once per frame
	void Update () {
        if (!testMode)
            image.rectTransform.sizeDelta = new Vector2(25 * (playerLives - 1), 33);
        if (!testMode || scoreText != null)
            scoreText.text = "SCORE: " + score;
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }

    public void SetPlayerLives(int lives)
    {
        playerLives = lives;
    }

    public int GetWavesCleared()
    {
        return wavesCleared;
    }

    void EnemySpawned()
    {
        numEnemies++;
    }

    public int GetNumEnemies()
    {
        return numEnemies;
    }

    void EnemyShipDestroyed(Enemy enemy)
    {
        score += enemy.score;
        numEnemies--;
    }

    void WavesClearedCounter()
    {
        wavesCleared++;
    }

    void PlayerShipDestroyed()
    {
        playerLives--;
    }

    public int GetScore()
    {
        return score;
    }
}
