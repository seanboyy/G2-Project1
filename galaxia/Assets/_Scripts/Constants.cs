using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Constants : MonoBehaviour {

    public Image image;
    public int playerLives = 5;

	// Use this for initialization
	void Start () {
        image.rectTransform.sizeDelta = new Vector2(25 * playerLives, 33);
	}
	
	// Update is called once per frame
	void Update () {
        image.rectTransform.sizeDelta = new Vector2(25 * playerLives, 33);
    }
}
