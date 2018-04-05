using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Constants : MonoBehaviour {

    // Singleton stuff
    public static Constants instance;

    public Image image;
    public int playerLives = 5;
    public Vector3 playerPos;
    public Text scoreText;

    [SerializeField]
    private int score = 0;


    void Awake()
    {
        if (instance == null)
            instance = this;
        Messenger<Enemy>.AddListener(Messages.ENEMY_DESTROYED, EnemyShipDestroyed);
    }

	// Use this for initialization
	void Start () {
        image.rectTransform.sizeDelta = new Vector2(25 * playerLives, 33);
	}
	
	// Update is called once per frame
	void Update () {
        image.rectTransform.sizeDelta = new Vector2(25 * playerLives, 33);
        if (scoreText != null)
            scoreText.text = "SCORE: " + score;
    }

    void EnemyShipDestroyed(Enemy enemy)
    {
        score += enemy.score;
    }

    public int getScore()
    {
        return score;
    }
}
