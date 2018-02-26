using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;



public class LocalPlayer : NetworkBehaviour {



	//[SyncVar (hook = "CmdChangeName")]public string pname = "player" + nid;
	[SyncVar] public string pname;
	[SyncVar] public string plost;

    [SyncVar] public bool tagged = false;
    [SyncVar] public bool canBeTagged = true;

    [SyncVar] public float lastTagged;
    [SyncVar] public float whenTagged;
    public float timeTagged = 0;

    [SyncVar] public bool lostGame = false;
	[SyncVar] public bool gameEnd = false;

    public float startTime = -1.0f;
    public bool tagSet = false;

    public Text loserText;

    private System.Object aLock = new System.Object();

	void OnGUI() {
		if (isLocalPlayer) {
			pname = GUI.TextField (new Rect (10, Screen.height - 40, 40, 50), pname);
			if (GUI.Button (new Rect (60, Screen.height - 40, Screen.height - 40, 50), "Change")) { 
				CmdChangeName (pname);
			}
		}
	}

	[Command] public void CmdChangeName(string newName) {
		this.pname = newName;
	}

	[Command] public void CmdGameEnd(string newLost) {
		this.plost = newLost;
	}

	[Server] public void CmdChangeTag() {
		this.tagged = !this.tagged;
        if (this.tagged) {
            whenTagged = Time.timeSinceLevelLoad;
        }
	}

	[Server] public void CmdCanBeTagged() {
		this.canBeTagged = !this.canBeTagged;

	}

    [Server] public void CmdTimeTag(float time) {
        this.lastTagged= time;
    }

	[Command] public void CmdLostGame(bool value) {
		lostGame = value;
	}
	// Use this for initialization
	void Start () {
		this.pname = "Player " + netId.ToString();
        this.loserText = GameObject.FindGameObjectWithTag("LoserText").GetComponent<Text>();
		if (isLocalPlayer) {
			GetComponent<Movement>().enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
		this.GetComponentInChildren<TextMesh> ().text = pname;
		/*loserText.text = plost;
        
		if (lostGame) {
			CmdGameEnd ("Player: " + pname + " Lost!");
			Debug.Log ("passed");
		}*/

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (netId.ToString() == "1" && players.Length > 1 && !tagSet) {
            lock (aLock) {
                if (startTime > 0 && Time.timeSinceLevelLoad - startTime > 5 && !tagSet) {
                    GameObject player = players[Random.Range(0, players.Length)];
                    player.GetComponent<LocalPlayer>().CmdChangeTag();
                    tagSet = true;
                }
                else if (startTime < 0) {
                    startTime = Time.timeSinceLevelLoad;
                }
            }
        } else {
            startTime = -1.0f;
        }

        if (isLocalPlayer && tagged) {
            timeTagged += Time.deltaTime;
            Debug.Log(pname + ": " + timeTagged.ToString());
            if (timeTagged > 10.0f) {
				CmdLostGame(true);
            }
		}

        if (this.tagged) {
            this.GetComponent<SpriteRenderer>().color = Color.green;
            this.GetComponent<Movement>().speed = 6;
        }
        else if (!this.canBeTagged) {
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
            this.GetComponent<Movement>().speed = 3;
        } else {
            this.GetComponent<SpriteRenderer>().color = Color.white;
            this.GetComponent<Movement>().speed = 3;
        }

        if (!this.canBeTagged) {
            if (lastTagged + 3 < Time.timeSinceLevelLoad) {
				this.CmdCanBeTagged();
            }
        }

    }


    [Server] private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            LocalPlayer lp = go.GetComponent<LocalPlayer>();

            if (this.tagged && lp.canBeTagged) {
                this.CmdCanBeTagged();
                this.CmdChangeTag();
                lp.GetComponent<LocalPlayer>().CmdCanBeTagged();
                lp.GetComponent<LocalPlayer>().CmdChangeTag();
                this.CmdTimeTag(Time.timeSinceLevelLoad);
            }

        }

    }


}
