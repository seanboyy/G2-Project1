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
    }

    // Use this for initialization
    void Start () {
        returnToMenuButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skySpinSpeed);
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
            if (FindObjectOfType<Player>() == null)
            {
                StartCoroutine("DelayPlayerSpawn");
            }
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
