using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class LocalPlayer : NetworkBehaviour {



	//[SyncVar (hook = "CmdChangeName")]public string pname = "player" + nid;
	[SyncVar] public string pname;
    [SyncVar] public bool tagged;
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

    [Command] public void CmdChangeTag(bool newTag) {
        tagged = newTag;
    }

    [Command] public void CmdCanBeTagged(bool value)
    {
        canBeTagged = value;
    }

    [Command] public void CmdTimeTag(float time)
    {
        lastTagged= time;
    }

	// Use this for initialization
	void Start () {
		this.pname = "Player " + netId.ToString();
		if (isLocalPlayer) {
			GetComponent<Movement>().enabled = true;

		}
	}

	// Update is called once per frame
	void Update () {
		
		this.GetComponentInChildren<TextMesh> ().text = pname;
        /*if (isLocalPlayer) {
			this.GetComponentInChildren<TextMesh> ().text = pname;
		}*/
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
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
        }

        if (this.tagged)
        {
            this.GetComponent<SpriteRenderer>().color = Color.green;
        } else {
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (!this.canBeTagged) {
            if (lastTagged + 5 < Time.timeSinceLevelLoad)
            {
                canBeTagged = true;
            }
        }
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            LocalPlayer lp = go.GetComponent<LocalPlayer>();

            if (lp.tagged && this.canBeTagged)
            {
                Debug.Log("Collision");
                lp.CmdCanBeTagged(false);
                this.CmdChangeTag(true);
                lp.CmdChangeTag(false);
                lp.CmdTimeTag(Time.timeSinceLevelLoad);
                Debug.Log(canBeTagged);
                Debug.Log(tagged);
                Debug.Log(pname);
            } else {
                Debug.Log("else");
            }        
        }

    }


}
