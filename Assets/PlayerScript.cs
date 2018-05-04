using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {
	// Use this for initialization

	private bool finishedTutorial = false;

	public class RegisterHostMessage : MessageBase
	{
		public string gameName;
		public string comment;
		public bool passwordProtected;
	}

	void Start () {
		if (!hasAuthority) {
			return;
		}

		if (isServer) {  //SERVER WILL BE TAGGER, CLIENT WILL BE TRASHER
			
		} else {
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasAuthority) {
			return;
		}

		if (isServer) {
			if (MakeWordBank.waitingForOtherPlayer) {
				RpcAskClientIfFinishedTutorial ();
				if (finishedTutorial) {
					MakeWordBank.otherPlayerHasFinished = true;
				}
			}
		} else {

		}
	}

	[ClientRpc]
	void RpcAskClientIfFinishedTutorial() {
		if (MakeWordBank.waitingForOtherPlayer) { //If client is already waiting
			MakeWordBank.otherPlayerHasFinished = true;
			finishedTutorial = true;
		}
	}


	/*
	[Command] bool askServerIfFinishedTutorial() {
		if (MakeWordBank.waitingForOtherPlayer) { //If server is already waiting
			MakeWordBank.otherPlayerHasFinished = true;
		}
	}
*/
}
