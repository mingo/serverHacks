using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour {

    public static GameObject taggedPlayer = null;
    public static bool tagged = false;
    public int countdown = -1;

    public float lastTagged;
    public bool canBeTagged = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length >= 2) {
            if (countdown == -1) {
                countdown = 5;
            } else if (countdown == 0 && taggedPlayer == null) {
                taggedPlayer = players[Random.Range(0, players.Length)];
                SpriteRenderer sr = taggedPlayer.GetComponent<SpriteRenderer>();
                sr.color = Color.green;
            } else if (countdown > 0) {
                countdown--;
            }
        } else {
            countdown = -1;
        }
	}

    private void Update()
    {
        if (lastTagged + 5 < Time.timeSinceLevelLoad) {
            canBeTagged = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        Tag tg = go.GetComponent<Tag>();

        if (go == taggedPlayer && tg.canBeTagged) {
            taggedPlayer = this.gameObject;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            go.GetComponent<SpriteRenderer>().color = Color.white;
            tg.lastTagged = Time.timeSinceLevelLoad;
            tg.canBeTagged = false;
        }
    }
}
