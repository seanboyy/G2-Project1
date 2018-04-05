using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies Spawned Bottom-Up")]
    public GameObject[] enemies;

    [Header("Set Dynamically")]
    public int enemiesAlive = 0;

    // Some constants to help make rows
    private int e0pRow = 7;
    private int e0spacing = 6; 
    private int e1pRow = 5;
    private int e1spacing = 8;
    private int e2pRos = 1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void SpawnRank()
    {

    }
}
