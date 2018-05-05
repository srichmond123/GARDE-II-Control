using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {
	// Use this for initialization

	private bool finishedTutorial = false;
	static int frame = 0;

	void Start () {
		if (!localPlayerAuthority) {
			return;
		}

		if (isServer) {  //SERVER WILL BE TAGGER, CLIENT WILL BE TRASHER
			
		} else {
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!localPlayerAuthority) {
			return;
		}
		frame++;
			
		if (frame > 5) { //This is done to not slow network traffic by calling commands every frame
			if (isServer) {
				if (MakeWordBank.waitingForOtherPlayer) {
					RpcAskClientIfFinishedTutorial ();

					//Debug.Log (MakeWordBank.waitingForOtherPlayer); //Is server still waiting???
				}
			} else {
				if (MakeWordBank.otherPlayerHasFinished) {
					CmdTellServerClientIsFinished ();
				}
			}
			frame = 0;
		}
	}

	[ClientRpc]
	void RpcAskClientIfFinishedTutorial() {
		if (!isServer) { //Server is a client too
			if (MakeWordBank.waitingForOtherPlayer) { //If client is already waiting
				MakeWordBank.otherPlayerHasFinished = true;
			}
		}
	}

	[Command]
	void CmdTellServerClientIsFinished() {
		MakeWordBank.otherPlayerHasFinished = true;
	}


	/*
	[Command] bool askServerIfFinishedTutorial() {
		if (MakeWordBank.waitingForOtherPlayer) { //If server is already waiting
			MakeWordBank.otherPlayerHasFinished = true;
		}
	}
*/
}
