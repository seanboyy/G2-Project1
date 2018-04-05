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

    public int GetPlayerLives()
    {
        return playerLives;
    }

    public void SetPlayerLives(int lives)
    {
        playerLives = lives;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        Messenger<Enemy>.AddListener(Messages.ENEMY_DESTROYED, EnemyShipDestroyed);
        Messenger.AddListener(Messages.PLAYER_DESTROYED, PlayerShipDestroyed);
    }

	// Use this for initialization
	void Start () {
        if (!testMode)
            image.rectTransform.sizeDelta = new Vector2(25 * playerLives, 33);
	}
	
	// Update is called once per frame
	void Update () {
        if (!testMode)
            image.rectTransform.sizeDelta = new Vector2(25 * playerLives, 33);
        if (!testMode || scoreText != null)
            scoreText.text = "SCORE: " + score;
    }

    void EnemyShipDestroyed(Enemy enemy)
    {
        score += enemy.score;
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
