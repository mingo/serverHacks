using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class LocalPlayer : NetworkBehaviour {



	//[SyncVar (hook = "CmdChangeName")]public string pname = "player" + nid;

	/*void OnGUI() {
		if (isLocalPlayer) {
			pname = GUI.TextField (new Rect (10, Screen.height - 40, 40, 50), pname);
			if (GUI.Button (new Rect (60, Screen.height - 40, Screen.height - 40, 50), "Change")) { 
				CmdChangeName (pname);
			}
		}
	}

	public void CmdChangeName(string newName){
		pname = newName;
	}*/

	[SyncVar] public string pname;

	// Use this for initialization
	void Start () {
		
		if (isLocalPlayer) {
			GetComponent<Movement>().enabled = true;

		}
	}

	// Update is called once per frame
	void Update () {
		this.pname = "Player " + netId.ToString();
		this.GetComponentInChildren<TextMesh> ().text = pname;
		/*if (isLocalPlayer) {
			this.GetComponentInChildren<TextMesh> ().text = pname;
		}*/
	}
}
