using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies Spawned Bottom-Up")]
    public GameObject[] enemies;

    [Header("Set Dynamically")]
    public int enemiesAlive = 0;

    public int row = 0;
    public int rowIncrement = 6;

    // Some constants to help make rows

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < enemies.Length; i++)
            SpawnRank(enemies[i]);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SpawnRank(GameObject enemyGO)
    {
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        if (enemy == null) return;
        int enemiesPerRow, enemySpacing;
        switch (enemy.rank)
        {
            case ShipRank.enemy_0:
                enemiesPerRow = 7;
                enemySpacing = 6;
                break;
            case ShipRank.enemy_1:
                enemiesPerRow = 5;
                enemySpacing = 8;
                break;
            case ShipRank.enemy_2:
                enemiesPerRow = 1;
                enemySpacing = 15;
                break;
            case ShipRank.NOT_CLASSIFIED:
                Debug.Log("Using ship where rank is not classified");
                enemiesPerRow = 0;
                enemySpacing = 0;
                break;
            default:
                enemiesPerRow = 0;
                enemySpacing = 0;
                break;
        }
        int i;
        if (enemiesPerRow % 2 != 0)
        {
            Debug.Log("doing weird odd stuff");
            i = -enemiesPerRow / 2;
            enemiesPerRow /= 2;
        }
        else
        {
            Debug.Log("enemiesPerRow " + enemiesPerRow);
            i = 0;
        }
        for (; i <= enemiesPerRow; i++)
        {
            Debug.Log(i);
            Vector3 rankPos = new Vector3(i * enemySpacing, row, 0);
            Instantiate(enemyGO, rankPos, new Quaternion()).GetComponent<Enemy_0>().rankPos = rankPos;
        }
        row += rowIncrement;
    }
}
