using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : NetworkBehaviour {

	[SyncVar]
	public string pname = "player";

	void OnGUI() {
		if (isLocalPlayer) {
			pname = GUI.TextField (new Rect (10, 10, Screen.height - 40, 100), pname);
			CmdChangeName (pname);

		}
	}
	[Command]
	public void CmdChangeName(string newName){
		pname = newName;
	}
	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<Movement>().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer)
			this.GetComponentInChildren<TextMesh> ().text = pname;
		
	}
}
