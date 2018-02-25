using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class LocalPlayer : NetworkBehaviour {



	//[SyncVar (hook = "CmdChangeName")]public string pname = "player" + nid;
	[SyncVar] public string pname;
    [SyncVar] public bool tagged = false;
    [SyncVar] public bool canBeTagged = true;
    [SyncVar] public float lastTagged;

    public float startTime = -1.0f;
    public bool tagSet = false;


	void OnGUI() {
		if (isLocalPlayer) {
			pname = GUI.TextField (new Rect (10, Screen.height - 40, 40, 50), pname);
			if (GUI.Button (new Rect (60, Screen.height - 40, Screen.height - 40, 50), "Change")) { 
				CmdChangeName (pname);
			}
		}
	}

	[Command] public void CmdChangeName(string newName){
		this.pname = newName;
	}

	[Command] public void CmdChangeTag1(bool value) {
		this.tagged = value;
		Debug.Log (pname + tagged.ToString());
    }

	[Command] public void CmdCanBeTagged1(bool value)
    {
		this.canBeTagged = value;

    }
	[Command] public void CmdChangeTag() {
		this.tagged = !this.tagged;
		Debug.Log (pname + tagged.ToString());
	}

	[Command] public void CmdCanBeTagged()
	{
		this.canBeTagged = !this.canBeTagged;

	}


    [Command] public void CmdTimeTag(float time)
    {
        this.lastTagged= time;
    }

	// Use this for initialization
	void Start () {
		this.pname = "Player " + netId.ToString();
		if (isLocalPlayer) {
			GetComponent<Movement>().enabled = true;
		}
		if (this.netId.ToString () == "1") {
			this.CmdChangeTag();
		}
	}

	// Update is called once per frame
	void Update () {
		
		this.GetComponentInChildren<TextMesh> ().text = pname;

        /*if (isLocalPlayer) {
			this.GetComponentInChildren<TextMesh> ().text = pname;
		}*/
		/*GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (netId.ToString() == "1" && players.Length > 1 && !tagSet) {
            if (startTime > 0 && Time.timeSinceLevelLoad - startTime > 5 && !tagSet) {
                GameObject player = players[Random.Range(0, players.Length)];
                player.GetComponent<LocalPlayer>().CmdChangeTag(true);
                tagSet = true;
                Debug.Log("chose");
            } else if (startTime < 0) {
                startTime = Time.timeSinceLevelLoad;
            }
        } else {
            startTime = -1.0f;
        }*/
		

        if (this.tagged)
        {
            this.GetComponent<SpriteRenderer>().color = Color.green;
			this.GetComponent<Movement> ().speed = 6;
        } else {
            this.GetComponent<SpriteRenderer>().color = Color.white;
			this.GetComponent<Movement> ().speed = 3;
        }

        if (!this.canBeTagged) {
            if (lastTagged + 5 < Time.timeSinceLevelLoad)
            {
				this.CmdCanBeTagged();
            }
        }
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		/*bool check1 = false;
		bool check2 = true;

		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetComponent<LocalPlayer> ().tagged) {
				check1 = true;
			} else {
				check2 = false;
			}
		}
		//if both are not tag, set player 1 to tag
		if (this.netId.ToString () == "1") {
			if (check1 == false) {
				this.CmdChangeTag1 (true);
			}
		}
		//if both are tag, player 2 is not tag
		if (this.netId.ToString () == "2") {
			if (check2 == true) {
				this.CmdChangeTag1 (false);
			}
		}*/

		if (players [0].GetComponent<LocalPlayer> ().tagged) {
			players [1].GetComponent<LocalPlayer> ().CmdChangeTag1 (false);
		} else {
			players [1].GetComponent<LocalPlayer> ().CmdChangeTag1 (true);
		}

	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            LocalPlayer lp = go.GetComponent<LocalPlayer>();

			/*if (this.netId.ToString () == "1" && lp.tagged && this.canBeTagged) {
				this.CmdCanBeTagged ();
				this.CmdChangeTag ();
			} else if (this.canBeTagged) {
				this.CmdCanBeTagged ();
				this.CmdChangeTag1 (lp.tagged);
			}*/

			if (lp.netId.ToString () == "1" && this.canBeTagged && lp.canBeTagged) {
				this.CmdCanBeTagged();
				this.CmdChangeTag1 (lp.tagged);
			}
			else{
				this.CmdCanBeTagged();
				this.CmdChangeTag();
			}

			
	

			/*if (lp.tagged && this.canBeTagged) { //if other player is tag and this player can be tagged
				Debug.Log ("Collision");
				this.CmdChangeTag (true); //this player is tag

				//this.canBeTagged = false;
				//Debug.Log (tagged);

			} 
			else if (this.tagged && lp.canBeTagged) {
				this.CmdChangeTag (false);
				this.CmdCanBeTagged(false);
				this.CmdTimeTag(Time.timeSinceLevelLoad);
				//this.lastTagged = Time.timeSinceLevelLoad; //time
				//this.canBeTagged = false; //other player cant be tagged
				//this.CmdChangeTag(false); //otherplayer is not tag
				//lp.CmdChangeTag(true);
				//lp.canBeTagged = false;
			}
			else {
				Debug.Log(pname);
            }*/        
        }

    }


}
