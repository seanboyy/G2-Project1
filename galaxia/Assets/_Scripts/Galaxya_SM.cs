using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Galaxya_SM : WytriamSTD.Scene_Manager {

    public GameObject playerPrefab;
    public GameObject returnToMenuButton;
    public float skySpinSpeed;

    private void Awake()
    {
        Messenger.AddListener(Messages.PLAYER_DESTROYED, SpawnPlayer);
        Messenger<Enemy>.AddListener(Messages.ENEMY_DESTROYED, CheckWaveClear);

    }

    // Use this for initialization
    void Start () {
        returnToMenuButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skySpinSpeed);
	}

    void CheckWaveClear(Enemy e)
    {
        if (Constants.instance.GetNumEnemies() <= 0)
        {
            StartCoroutine("NewWave");
        }
    }

    IEnumerator NewWave()
    {
        announce("WAVE CLEAR");
        yield return new WaitForSeconds(3);
        Messenger.Broadcast(Messages.WAVE_CLEAR);
        yield return null;
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
            StartCoroutine("DelayPlayerSpawn");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    IEnumerator DelayPlayerSpawn()
    {
        yield return new WaitForSeconds(0.5F);
        Instantiate(playerPrefab).GetComponent<Player>().IsInvulnerable = true;
        yield return null;
    }
}
