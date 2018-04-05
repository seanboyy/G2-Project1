using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxya_SM : WytriamSTD.Scene_Manager {

    public GameObject playerPrefab;

    private void Awake()
    {
        Messenger.AddListener(Messages.PLAYER_DESTROYED, SpawnPlayer);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnPlayer()
    {
        if(Constants.instance.GetPlayerLives() <= 0)
        {
            announce("GAME OVER");
        }
        else
        {
            Instantiate(playerPrefab);
        }
    }
}
