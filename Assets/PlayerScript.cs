using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {
	// Use this for initialization

	private static bool finishedWaiting = false; //Once this is true, client and server should be playing together and constantly sending signals back and forth
	static int frame = 0;

//	static GameObject taggerPanel;
//	static GameObject trasherPanel;

	static bool taggerPanelIsSet = false;
	static bool trasherPanelIsSet = false;

	void Start () {
		if (!localPlayerAuthority) {
			return;
		}

		/*
		taggerPanel.transform.Translate (new Vector3 (0, 5000, 0)); //Moving it out of the way for tutorial
		trasherPanel.transform.Translate (new Vector3 (0, 5000, 0));

		*/

		if (isServer) {  //Server is tagger, client is trasher
			
		} else {
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!localPlayerAuthority) {
			return;
		}
		frame++;
			
		if (frame > 3) { //This is done to not slow network traffic by calling commands every frame
			if (isServer) {
				if (MakeWordBank.waitingForOtherPlayer) {
					RpcAskClientIfFinishedTutorial ();
				}

				if (finishedWaiting) { //Playing the game:
					if (!taggerPanelIsSet) {
						MakeWordBank.taggerPanel.transform.Translate (new Vector3 (0, -5000, 0));
						taggerPanelIsSet = true;
					}
				}
			} else {
				if (MakeWordBank.otherPlayerHasFinished) {
					CmdTellServerClientIsFinished ();
				}
				if (finishedWaiting) {
					if (!trasherPanelIsSet) {
						MakeWordBank.trasherPanel.transform.Translate (new Vector3 (0, -5000, 0));
						trasherPanelIsSet = true;
					}
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
				finishedWaiting = true;
			}
		}
	}

	[Command]
	void CmdTellServerClientIsFinished() {
		MakeWordBank.otherPlayerHasFinished = true;
		finishedWaiting = true;
	}


	/*
	[Command]
	void CmdShiftPanels() {
		taggerPanel.transform.Translate (new Vector3 (0, 5000, 0));
		trasherPanel.transform.Translate (new Vector3 (0, 5000, 0));
	}

	[ClientRpc]
	void RpcShiftPanels() {
		taggerPanel.transform.Translate (new Vector3 (0, 5000, 0));
		trasherPanel.transform.Translate (new Vector3 (0, 5000, 0));
	}
	*/
}
