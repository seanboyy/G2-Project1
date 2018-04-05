﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Galaxya_SM : WytriamSTD.Scene_Manager {

    public GameObject playerPrefab;
    public GameObject returnToMenuButton;

    private void Awake()
    {
        Messenger.AddListener(Messages.PLAYER_DESTROYED, SpawnPlayer);
    }

    // Use this for initialization
    void Start () {
        returnToMenuButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnPlayer()
    {
        if(Constants.instance.GetPlayerLives() <= 0)
        {
            announce("GAME OVER");
            returnToMenuButton.SetActive(true);
        }
        else
        {
            Instantiate(playerPrefab);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
