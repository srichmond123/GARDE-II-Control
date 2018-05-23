using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class MakeWordBank : MonoBehaviour {

    //I'm putting the CSV files in the directory Assets/Tags, 
    //Using the final wordbank Roni gave me:
    
    public TextAsset tagsText;
	private List<GameObject> tagGameObjects;
	StateManager state; //For tutorial only

    // The Text object will be a child of the panel representing that tag, so it is fine to have one array representing the GameObject and the Tag for each tag
	//public Text[] textObjects;

	//Numbers 0-47 (elements of .csv file) in a random order, different predetermined random indexes for all 51 images (first is practice img):
	public static int[,] SEQUENCE = new int[,] { {
			44, 43, 2, 23, 30, 46, 3, 27, 45, 17, 41, 1, 42, 29, 12, 34, 16, 6, 13, 37, 47, 25, 21, 22, 19, 33, 32, 11, 26, 7, 24, 0, 10, 39, 8, 36, 20, 4, 31, 14, 40, 35, 18, 38, 15, 28, 9, 5
		}, {
			18, 21, 46, 10, 31, 24, 26, 13, 42, 47, 38, 45, 8, 5, 32, 29, 23, 40, 9, 11, 34, 0, 25, 39, 41, 17, 12, 2, 27, 7, 37, 1, 4, 16, 43, 30, 33, 3, 36, 14, 20, 28, 6, 35, 22, 15, 44, 19
		}, {
			8, 45, 27, 30, 33, 0, 37, 32, 6, 36, 40, 14, 16, 46, 10, 13, 4, 21, 18, 9, 11, 1, 47, 15, 12, 7, 2, 35, 38, 31, 17, 29, 39, 24, 44, 25, 23, 20, 43, 28, 5, 22, 42, 26, 3, 41, 19, 34
		}, {
			20, 15, 40, 23, 3, 7, 2, 22, 36, 0, 35, 17, 26, 28, 41, 18, 47, 19, 34, 46, 4, 5, 12, 39, 24, 1, 42, 14, 38, 16, 30, 31, 44, 25, 10, 37, 9, 21, 6, 29, 45, 32, 11, 27, 13, 8, 43, 33
		}, {
			14, 45, 3, 11, 28, 2, 9, 38, 22, 13, 27, 23, 26, 32, 0, 19, 40, 10, 36, 6, 4, 21, 37, 29, 18, 46, 39, 8, 43, 24, 20, 31, 30, 34, 15, 7, 17, 16, 33, 42, 41, 25, 44, 47, 12, 1, 35, 5
		}, {
			9, 36, 14, 18, 35, 26, 34, 33, 19, 21, 38, 32, 30, 15, 44, 16, 24, 7, 47, 13, 29, 4, 31, 28, 42, 3, 5, 12, 17, 25, 11, 1, 39, 2, 46, 27, 0, 45, 6, 43, 20, 41, 10, 22, 23, 40, 8, 37
		}, {
			39, 0, 33, 6, 17, 27, 7, 16, 2, 8, 15, 12, 34, 5, 32, 40, 30, 37, 29, 43, 36, 19, 35, 41, 10, 42, 9, 14, 23, 28, 26, 44, 38, 47, 3, 4, 1, 22, 31, 20, 46, 13, 25, 11, 18, 21, 24, 45
		}, {
			47, 28, 45, 12, 4, 31, 40, 33, 39, 13, 41, 8, 18, 2, 35, 27, 1, 25, 36, 16, 11, 14, 17, 15, 46, 7, 38, 42, 44, 6, 43, 30, 24, 20, 37, 23, 10, 29, 21, 34, 9, 5, 26, 22, 32, 19, 3, 0
		}, {
			28, 23, 5, 27, 9, 17, 8, 38, 32, 40, 41, 15, 10, 16, 25, 11, 31, 7, 19, 36, 2, 20, 3, 46, 45, 43, 22, 42, 21, 26, 1, 35, 33, 47, 30, 6, 44, 24, 18, 14, 39, 29, 4, 13, 34, 37, 12, 0
		}, {
			21, 1, 47, 5, 42, 7, 0, 44, 36, 11, 18, 27, 12, 39, 23, 15, 24, 4, 40, 26, 28, 17, 45, 22, 10, 38, 19, 34, 35, 8, 13, 14, 41, 30, 43, 3, 6, 46, 32, 31, 9, 20, 29, 33, 2, 25, 16, 37
		}, {
			21, 3, 33, 16, 18, 29, 13, 34, 14, 41, 45, 24, 43, 20, 19, 7, 27, 6, 8, 37, 1, 11, 36, 15, 30, 31, 26, 10, 9, 47, 4, 32, 28, 38, 25, 2, 17, 46, 40, 23, 42, 35, 44, 39, 12, 5, 22, 0
		}, {
			5, 25, 42, 31, 29, 13, 27, 21, 41, 0, 33, 47, 16, 11, 7, 30, 10, 39, 46, 37, 20, 9, 2, 3, 34, 36, 45, 4, 14, 12, 35, 26, 44, 28, 19, 38, 24, 8, 17, 15, 23, 32, 22, 1, 18, 40, 6, 43
		}, {
			44, 47, 11, 40, 15, 33, 42, 17, 6, 2, 36, 24, 31, 37, 23, 14, 18, 39, 13, 3, 16, 29, 0, 9, 28, 7, 46, 22, 35, 45, 41, 1, 30, 34, 25, 5, 20, 19, 8, 21, 4, 26, 10, 38, 43, 32, 12, 27
		}, {
			4, 33, 15, 25, 10, 6, 29, 9, 34, 3, 24, 45, 30, 18, 42, 46, 28, 32, 5, 7, 20, 19, 31, 0, 37, 36, 12, 27, 40, 44, 26, 38, 41, 13, 22, 23, 35, 2, 39, 17, 16, 14, 47, 21, 8, 1, 11, 43
		}, {
			5, 10, 43, 18, 44, 15, 36, 6, 40, 0, 23, 47, 32, 35, 30, 11, 4, 14, 12, 34, 20, 21, 28, 2, 33, 1, 27, 7, 42, 19, 13, 37, 29, 22, 26, 38, 24, 17, 16, 41, 25, 3, 46, 31, 9, 8, 39, 45
		}, {
			0, 42, 7, 26, 41, 27, 36, 33, 11, 12, 18, 35, 25, 45, 29, 15, 40, 19, 21, 16, 5, 47, 44, 14, 23, 43, 4, 13, 10, 37, 39, 24, 28, 30, 6, 20, 38, 9, 17, 31, 32, 2, 34, 22, 1, 46, 8, 3
		}, {
			7, 47, 46, 34, 31, 38, 41, 44, 12, 35, 6, 18, 37, 2, 9, 33, 40, 42, 13, 27, 15, 5, 36, 30, 20, 21, 8, 4, 16, 28, 23, 22, 29, 19, 0, 24, 17, 39, 3, 43, 26, 25, 10, 45, 32, 14, 11, 1
		}, {
			17, 7, 27, 11, 2, 22, 43, 31, 35, 42, 23, 44, 40, 38, 30, 39, 1, 21, 45, 6, 18, 26, 16, 14, 9, 19, 10, 32, 3, 25, 47, 20, 15, 28, 0, 33, 29, 46, 8, 37, 5, 4, 24, 41, 36, 34, 13, 12
		}, {
			31, 23, 43, 2, 3, 18, 37, 0, 45, 44, 30, 26, 11, 1, 5, 29, 14, 24, 13, 22, 47, 40, 32, 6, 41, 39, 7, 17, 12, 42, 10, 38, 15, 46, 20, 8, 4, 28, 9, 27, 19, 33, 16, 35, 36, 34, 25, 21
		}, {
			20, 29, 17, 9, 4, 11, 22, 42, 14, 3, 31, 36, 47, 23, 21, 37, 26, 1, 7, 16, 41, 33, 30, 2, 24, 10, 40, 27, 44, 43, 0, 39, 32, 46, 34, 38, 45, 28, 6, 15, 19, 8, 12, 13, 18, 25, 5, 35
		}, {
			18, 47, 31, 15, 27, 25, 12, 7, 6, 26, 28, 14, 23, 11, 30, 20, 43, 3, 32, 8, 45, 1, 22, 41, 19, 37, 24, 39, 10, 36, 34, 38, 16, 4, 9, 40, 21, 17, 42, 46, 35, 2, 13, 5, 44, 29, 0, 33
		}, {
			45, 22, 5, 7, 21, 3, 46, 0, 2, 29, 42, 14, 35, 33, 13, 43, 39, 44, 27, 12, 41, 19, 30, 34, 40, 1, 16, 31, 26, 47, 11, 36, 4, 28, 17, 25, 8, 23, 37, 20, 32, 6, 15, 38, 10, 9, 24, 18
		}, {
			43, 7, 29, 14, 10, 27, 32, 11, 30, 17, 8, 2, 0, 18, 19, 9, 24, 41, 33, 25, 15, 23, 31, 39, 38, 46, 44, 35, 13, 40, 45, 21, 5, 4, 20, 34, 22, 26, 47, 42, 1, 28, 37, 16, 6, 12, 36, 3
		}, {
			18, 0, 38, 41, 8, 17, 21, 46, 10, 29, 40, 1, 5, 12, 47, 9, 30, 25, 31, 6, 20, 7, 27, 16, 23, 24, 44, 11, 43, 37, 35, 33, 42, 26, 14, 39, 22, 19, 3, 4, 13, 32, 28, 15, 36, 2, 34, 45
		}, {
			35, 11, 32, 38, 33, 28, 2, 27, 45, 18, 44, 34, 23, 10, 43, 36, 8, 15, 3, 17, 7, 46, 9, 19, 42, 39, 29, 47, 14, 22, 5, 12, 24, 37, 30, 20, 1, 21, 25, 40, 0, 41, 31, 16, 13, 4, 26, 6
		}, {
			41, 3, 35, 22, 14, 10, 5, 25, 11, 47, 9, 16, 38, 0, 44, 18, 37, 45, 4, 36, 32, 1, 33, 46, 34, 43, 17, 12, 19, 8, 15, 7, 6, 24, 21, 2, 39, 30, 27, 26, 23, 20, 29, 28, 40, 13, 31, 42
		}, {
			32, 44, 23, 0, 11, 13, 27, 1, 34, 45, 15, 39, 40, 28, 20, 31, 26, 4, 36, 33, 21, 37, 42, 24, 47, 9, 16, 2, 5, 43, 25, 18, 14, 41, 17, 19, 38, 12, 3, 6, 10, 35, 7, 8, 22, 30, 29, 46
		}, {
			34, 0, 20, 47, 41, 12, 26, 27, 13, 14, 18, 8, 45, 6, 11, 15, 7, 10, 16, 44, 1, 5, 4, 9, 21, 31, 39, 22, 37, 46, 32, 33, 2, 28, 23, 25, 36, 43, 30, 17, 40, 38, 42, 24, 3, 29, 19, 35
		}, {
			14, 17, 24, 27, 45, 33, 42, 12, 0, 11, 8, 19, 4, 35, 36, 47, 44, 20, 3, 39, 23, 29, 25, 6, 18, 15, 5, 22, 2, 37, 41, 21, 26, 32, 13, 10, 1, 30, 34, 38, 43, 46, 28, 40, 31, 16, 7, 9
		}, {
			13, 18, 26, 29, 27, 5, 35, 44, 24, 45, 17, 15, 8, 14, 42, 19, 3, 10, 20, 34, 28, 47, 30, 16, 38, 39, 7, 2, 11, 25, 46, 9, 23, 21, 43, 31, 37, 36, 41, 6, 32, 12, 0, 4, 22, 1, 33, 40
		}, {
			31, 12, 7, 3, 18, 38, 21, 42, 35, 1, 37, 23, 11, 46, 34, 41, 26, 22, 6, 30, 27, 16, 8, 10, 45, 40, 2, 4, 15, 47, 24, 44, 28, 5, 20, 36, 9, 39, 33, 32, 29, 0, 19, 14, 43, 25, 13, 17
		}, {
			41, 30, 14, 17, 11, 44, 15, 42, 35, 7, 23, 25, 31, 37, 21, 9, 34, 27, 19, 43, 32, 36, 47, 18, 26, 45, 22, 13, 1, 40, 10, 4, 20, 39, 24, 3, 5, 8, 29, 6, 12, 0, 2, 16, 38, 46, 28, 33
		}, {
			40, 8, 29, 5, 9, 11, 33, 12, 14, 15, 10, 31, 4, 45, 23, 27, 17, 13, 7, 26, 19, 22, 35, 36, 47, 34, 24, 25, 38, 42, 2, 30, 41, 6, 37, 21, 3, 1, 32, 20, 43, 44, 0, 18, 16, 39, 46, 28
		}, {
			26, 45, 9, 13, 29, 7, 15, 37, 6, 25, 1, 21, 18, 8, 11, 32, 43, 35, 33, 10, 3, 47, 22, 27, 5, 14, 24, 23, 34, 4, 19, 12, 2, 44, 41, 30, 40, 46, 16, 28, 39, 20, 36, 31, 17, 38, 0, 42
		}, {
			16, 30, 44, 22, 33, 3, 26, 43, 31, 11, 13, 17, 14, 23, 21, 38, 18, 7, 27, 25, 8, 34, 47, 20, 28, 46, 10, 40, 35, 4, 6, 32, 37, 42, 9, 36, 39, 1, 2, 5, 15, 0, 12, 29, 41, 19, 45, 24
		}, {
			31, 6, 44, 39, 41, 28, 47, 10, 17, 18, 5, 22, 19, 2, 25, 14, 33, 27, 11, 30, 9, 13, 23, 16, 45, 29, 38, 36, 37, 26, 35, 21, 32, 4, 40, 42, 34, 20, 1, 7, 43, 3, 46, 15, 24, 0, 8, 12
		}, {
			15, 7, 9, 23, 20, 30, 3, 43, 34, 44, 41, 40, 28, 27, 42, 8, 19, 2, 47, 0, 6, 22, 46, 18, 36, 16, 11, 35, 10, 29, 13, 4, 38, 32, 14, 45, 26, 21, 39, 1, 33, 31, 5, 24, 25, 12, 37, 17
		}, {
			28, 45, 9, 23, 29, 6, 33, 13, 19, 20, 4, 25, 34, 32, 27, 14, 22, 30, 35, 17, 24, 42, 16, 1, 2, 31, 40, 5, 26, 11, 18, 15, 46, 41, 0, 39, 8, 36, 43, 12, 44, 10, 37, 3, 7, 38, 21, 47
		}, {
			3, 25, 37, 24, 19, 8, 9, 46, 31, 35, 47, 21, 44, 10, 41, 7, 17, 30, 0, 34, 13, 12, 22, 39, 38, 15, 29, 42, 45, 18, 2, 43, 14, 32, 26, 11, 16, 23, 40, 5, 1, 6, 4, 36, 27, 20, 28, 33
		}, {
			18, 39, 44, 14, 17, 4, 12, 23, 20, 38, 22, 37, 24, 31, 25, 13, 3, 28, 42, 32, 2, 10, 19, 8, 15, 16, 40, 47, 0, 30, 43, 11, 5, 29, 35, 9, 45, 34, 41, 6, 7, 33, 27, 1, 21, 26, 46, 36
		}, {
			9, 41, 12, 40, 29, 1, 28, 2, 3, 13, 4, 43, 25, 0, 39, 35, 36, 45, 44, 21, 23, 17, 42, 11, 10, 34, 7, 47, 6, 18, 19, 32, 15, 30, 20, 31, 14, 24, 27, 8, 33, 26, 5, 46, 22, 37, 16, 38
		}, {
			20, 3, 28, 4, 31, 30, 13, 8, 7, 6, 17, 16, 33, 10, 22, 19, 15, 43, 24, 14, 25, 23, 26, 5, 1, 34, 41, 37, 40, 21, 38, 39, 29, 2, 32, 12, 35, 47, 45, 11, 36, 46, 9, 18, 27, 0, 44, 42
		}, {
			9, 28, 19, 43, 34, 31, 18, 39, 41, 44, 22, 1, 45, 12, 23, 38, 27, 14, 37, 40, 20, 0, 42, 2, 46, 47, 3, 4, 5, 26, 25, 8, 7, 32, 10, 21, 16, 15, 11, 29, 17, 24, 35, 36, 33, 13, 30, 6
		}, {
			27, 19, 25, 20, 31, 21, 37, 17, 11, 13, 9, 41, 28, 6, 15, 0, 22, 3, 35, 14, 33, 32, 40, 5, 26, 1, 2, 38, 23, 47, 42, 44, 39, 36, 34, 29, 16, 46, 30, 4, 12, 7, 18, 8, 10, 24, 45, 43
		}, {
			18, 25, 11, 20, 16, 6, 36, 8, 5, 45, 27, 28, 21, 44, 34, 40, 0, 3, 38, 22, 14, 37, 15, 26, 2, 31, 29, 24, 33, 32, 4, 10, 12, 17, 42, 47, 13, 46, 7, 1, 30, 39, 23, 41, 9, 19, 35, 43
		}, {
			1, 8, 4, 36, 3, 18, 26, 29, 0, 14, 45, 24, 44, 12, 23, 30, 15, 43, 47, 20, 31, 19, 21, 6, 39, 22, 40, 28, 9, 33, 7, 32, 10, 42, 38, 46, 37, 17, 34, 5, 16, 25, 27, 35, 11, 13, 2, 41
		}, {
			45, 0, 24, 33, 41, 1, 32, 34, 12, 27, 36, 30, 28, 17, 5, 35, 9, 23, 11, 7, 6, 19, 40, 4, 3, 10, 38, 42, 39, 16, 15, 18, 44, 29, 31, 26, 43, 47, 25, 14, 21, 13, 22, 20, 37, 2, 8, 46
		}, {
			13, 35, 23, 28, 2, 22, 34, 37, 8, 36, 24, 33, 25, 12, 32, 30, 47, 16, 15, 41, 39, 19, 4, 40, 18, 5, 11, 10, 9, 3, 46, 44, 17, 29, 14, 42, 45, 26, 6, 38, 21, 31, 0, 20, 1, 43, 27, 7
		}, {
			7, 18, 11, 34, 17, 46, 6, 8, 5, 39, 45, 37, 1, 33, 28, 42, 13, 30, 20, 19, 29, 9, 12, 38, 36, 25, 26, 23, 27, 24, 0, 41, 31, 3, 10, 15, 44, 22, 16, 47, 21, 2, 40, 32, 4, 43, 14, 35
		}, {
			21, 33, 41, 20, 28, 26, 10, 18, 19, 40, 0, 35, 45, 25, 42, 16, 23, 17, 29, 5, 12, 32, 1, 13, 30, 27, 8, 43, 34, 44, 46, 15, 4, 37, 22, 3, 39, 9, 14, 31, 6, 36, 24, 11, 2, 47, 38, 7
		}, {
			18, 25, 11, 20, 16, 6, 36, 8, 5, 42, 27, 28, 21, 44, 34, 40, 0, 3, 38, 22, 14, 37, 15, 26, 2, 31, 29, 24, 33, 32, 4, 10, 12, 17, 45, 47, 13, 46, 7, 1, 30, 39, 23, 41, 9, 19, 35, 43
		}
	};

	static string[] tutorialWords = {
		"Woman","Police Car", "Screen", "Building", "Sky", "Cone", "Newspaper", "Mountain",
		"Man", "Chair", "Table", "Rail", "Vent", "Billboard", "Truck"
	};
	static int tutorialWordsIndex = 0;
	public static int sequenceIndex = 0; //Indicates which element of predetermined sequence we're on, should reset to 0 every turnover
	static int imageIndex = 0; //Index for which 360 Material to use as well as which predetermined random int sequence to use

	static int numTagsRemaining = 5;


	public static GameObject tagSphere;
	public Material[] imageMaterialsToDragIn;
	public Material tutorialImageMaterialDragFromEditor;
	public static Material tutorialImageMaterial;
	public static Material[] imageMaterials;

	public static Text tagsRemainingText;

	public static List<string> wordBank = new List<string>();
	public static GameObject focusor;
	public static GameObject tutorialArrow;
	public static GameObject secondTutorialArrow; //Just for showing select buttons step (need 2 arrows)
	public static GameObject falconHelper; //So when the focus window goes to the button it doesn't depend on absolute coordinates
	public static Text tutorialText;
	public static GameObject helpTextContainer;
    public static GameObject helpTextPanel;
	public static Text welcomeText;
	public static GameObject practiceLevelText;
	public static GameObject welcomeScreen;
	public static GameObject dataCollector;
	public static Vector3 positionLastTag, rotationLastTag, scaleLastTag; //For use in PlayerScript when the last tag of an image is dropped:

	public static bool inTutorial = false;
	public static bool inPracticeLevel = false;
	public static int stepOfTutorial = 0;
	public static float timePannedInTutorial = 0f;
	public static float timeSpentOnStep8 = 0f;
	public static float timeSpentBeforeFocus = 0f; //I don't want the step of demonstrating focus to possibly end instantly, so I'll make it show up for a mandatory minimum of time ~2 sec
	public static bool switchRight = true; //For the Select button part of the tutorial
	public static bool inScreenBeforeExperiment = false;
	public static bool waitingForOtherPlayer = false;
	public static bool otherPlayerHasFinished = false;
	public static float timeSpentAfterSurvey = 0.5f;

	public static bool skipTaggingTutorialStep = false;
	public static bool skipTrashingTutorialStep = false;
	public static bool continueAfterOtherQuit = false;

	public static GameObject taggerPanel, trasherPanel;

    int buttons;
    int buttonsPrev;

	//Array of the container class I made below for a "Tag" object - since it's static, 
	//you can have an eventlistener on another class and call methods like MakeWordBank.replaceTag(GameObject obj)
	//which replaces the Tag with the next Tag name in line, uploaded from the .csv file.
	//This script should work fine, the important thing is that the Text objects whose parents are the
	//tag GameObjects should have unique names (doesn't matter what the names are), the parent
	//GameObjects' names can be changed though with no problem
	public static Tag[] tags;

	void Start () {

        tagSphere = GameObject.FindGameObjectWithTag ("TagSphere");
		imageMaterials = new Material[imageMaterialsToDragIn.Length];
		tutorialImageMaterial = tutorialImageMaterialDragFromEditor;
		focusor = GameObject.FindGameObjectWithTag ("Focusor"); //Just used for step where user picks a tag
		focusor.SetActive(false);

		falconHelper = GameObject.FindGameObjectWithTag ("FalconHelper");
		state = GameObject.Find("Canvas").GetComponent<StateManager>();
		tutorialArrow = GameObject.Find ("TutorialArrow");
		secondTutorialArrow = GameObject.Find ("SecondTutorialArrow");
		secondTutorialArrow.SetActive (false);
		tutorialArrow.SetActive (false);
		tutorialText = GameObject.FindGameObjectWithTag ("TutorialText").GetComponent<Text> () as Text;
		tutorialText.text = ""; //Blank for now since welcome screen must come first
		helpTextContainer = GameObject.Find("HelpTextContainer");
		helpTextContainer.SetActive (false);
		welcomeText = GameObject.FindGameObjectWithTag ("WelcomeText").GetComponent<Text> () as Text;
		welcomeScreen = GameObject.Find ("WelcomeScreenPanel");
		practiceLevelText = GameObject.Find ("PracticeLevelText");
		helpTextPanel = tutorialText.transform.parent.gameObject;
		practiceLevelText.SetActive (false);

		taggerPanel = GameObject.FindGameObjectWithTag("TaggerPanel");
		trasherPanel = GameObject.FindGameObjectWithTag("TrasherPanel");

		taggerPanel.transform.Translate (new Vector3 (0, 5000, 0)); //Moving it out of the way for tutorial
		trasherPanel.transform.Translate (new Vector3 (0, 5000, 0));

		for (int i = 0; i < imageMaterials.Length; i++) {
			imageMaterials [i] = imageMaterialsToDragIn [i];
		}
		tagsRemainingText = GameObject.FindGameObjectWithTag ("TagsRemainingText").GetComponent<Text>();

        tagGameObjects = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child != transform) // The first child will be the parent transform, which should be excluded
            {
                tagGameObjects.Add(child.gameObject);
            }
        }

		tags = new Tag[tagGameObjects.Count];
		for (int i = 0; i < tags.Length; i++) {
			tags [i] = new Tag (tagGameObjects [i], i);
		}
		//Read CSV File:

		using (StringReader sr = new StringReader(tagsText.text))
		{
			string line;

			while ((line = sr.ReadLine()) != null)
			{
				string[] parts = line.Split(',');

				string elem = parts[parts.Length - 1]; //Last column of .csv must be the tag names
				if (!string.Equals (elem, "")) {
					wordBank.Add (elem);
				}
			}
		}
		wordBank.RemoveAt (0); //<-- Column name

		// **************** TUTORIAL: ******************************
		///
		/// 
		//////// 
		dataCollector = GameObject.FindGameObjectWithTag("DataCollector"); //Turn off data collection for tutorial
		dataCollector.SetActive(false);

		tagSphere.GetComponent<Renderer> ().material = tutorialImageMaterial;
		//Word bank isn't applicable for the tutorial level:
		for (int i = 0; i < tags.Length; i++) {
			tags[i].setText(tutorialWords[tutorialWordsIndex]);
			tutorialWordsIndex++;
		}
		////////
		/// 
		/// 
		// **************** IMAGE 1: *******************************
		/*for (int i = 0; i < tags.Length; i++) {
			tags[i].setText(wordBank [ SEQUENCE[imageIndex, sequenceIndex] ]);
			sequenceIndex++;
		}
		tagSphere.GetComponent<Renderer> ().material = imageMaterials [imageIndex];*/
	}
	
	// Update is called once per frame
	void Update () {
		//To add:
		//Survey,
		//Beginning ppt slides

		if (inTutorial) {
			buttons = state.getButtons ();
			if (stepOfTutorial == 0) { //Welcome screen step:
				timeSpentAfterSurvey += Time.deltaTime;
				if (buttons > 0 && timeSpentAfterSurvey >= 1f) { //Move to the next step (change for falcon):
					welcomeScreen.SetActive (false);
					tutorialArrow.SetActive (true);
					helpTextContainer.SetActive (true);
					//Change the size of the box
					tutorialText.text = "Press this button to pan around the image";
					//Width from 150->218
					//228,24
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (270, 24);
					tutorialText.GetComponent<RectTransform> ().sizeDelta 
					= new Vector2 (260, 65);
					tutorialText.transform.localPosition = new Vector2 (tutorialText.transform.localPosition.x, -25);
					helpTextContainer.transform.localPosition = new Vector3 (-215f, 150f, 0f);
					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 1) { //Panning step. The user will have to pan for 3 seconds to get to the next step:
				if ((buttons & 1) > 0) { //<-- if the right mouse button is held down (change for falcon):
					timePannedInTutorial += Time.deltaTime;
				}
				if (timePannedInTutorial >= 3.0f) {
					tutorialArrow.transform.localPosition = new Vector3 (132f, -120f, 0f); //To view cone
					helpTextContainer.transform.localPosition = new Vector3 (-200f, -90f, 0f);
					tutorialText.fontSize = 12;
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (430, 40f);
					tutorialText.text = "If you push or pull your controller too much, the image will fade out.\n" +
					"Try it to see.";
					tutorialText.GetComponent<RectTransform> ().sizeDelta 
					= new Vector2 (450, 65);
					tutorialText.transform.localPosition 
					= new Vector3 (tutorialText.transform.localPosition.x + 10, -15f, 0);

					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 2) { //Step for losing focus:
				timeSpentBeforeFocus += Time.deltaTime;
				if (timeSpentBeforeFocus >= 2f) {
					if (Mathf.Abs (DarkenerScript.scrollPos) * 3 > 0.33) { //User pushed in or out the controller far enough:
						//Go to next step:
						secondTutorialArrow.SetActive (true); //Used to show both select buttons
						tutorialArrow.transform.localPosition = new Vector3 (-83f, 134f, 0f);
						helpTextContainer.transform.localPosition = new Vector3 (-230f, 130f, 0f);
						tutorialText.GetComponent<RectTransform> ().sizeDelta 
						= new Vector2 (480, 65);
						helpTextPanel.GetComponent<RectTransform> ().sizeDelta
						= new Vector2 (480, 40);
						tutorialText.text = "Either one of these two buttons on your controller can be used " +
						"to select tags.\nPress either to continue.";
						stepOfTutorial++;
					}
				}
			} else if (stepOfTutorial == 3) { //Step where user is shown Select buttons:
				if ((buttons & 2) > 0) { //If I click the left mouse button down (change for falcon):
					if (!skipTaggingTutorialStep) {
						secondTutorialArrow.SetActive (false);
						tutorialArrow.SetActive (false);
						focusor.SetActive (true);
						helpTextContainer.transform.localPosition = new Vector3 (-150, 97, 0);
						tutorialText.text = "Press either button once (do not hold) " +
						"to select a tag";
						tutorialText.GetComponent<RectTransform> ().sizeDelta 
						= new Vector2 (330, 65);
						helpTextPanel.GetComponent<RectTransform> ().sizeDelta
						= new Vector2 (330, 20f);
						tutorialText.transform.localPosition 
						= new Vector3 (0, -25f, 0);
						stepOfTutorial++;
					} else {
						secondTutorialArrow.SetActive (false);
						stepOfTutorial += 2;
					}
				}
				
			} else if (stepOfTutorial == 4) { //Step where user selects highlighted tags:
				if (state.getSelected () != null) { //User's holding a tag, go to the next step:
					focusor.transform.localPosition = new Vector3 (-100.7f, -450f, -271.39f);
					focusor.transform.localScale = new Vector3 (30.7f, 8.2f, 3f);
					helpTextContainer.transform.localPosition = new Vector3 (30, 97, 0);
					tutorialText.GetComponent<RectTransform> ().sizeDelta 
					= new Vector2 (380, 65);
					tutorialText.text = "Place the tag on the image " +
					"by pressing the button once again";
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (380, 20f);
					tutorialText.transform.localPosition 
					= new Vector3 (12, -25f, 0);
					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 5) { //Step where user places the tag anywhere in the image:
				if (state.getSelected () == null) { //User dropped tag on image, switch to next tutorial step:
					focusor.SetActive (false);
					helpTextContainer.transform.localPosition = new Vector3 (50, 97, 0);
					tutorialArrow.SetActive (true);
					tutorialArrow.transform.localPosition = new Vector3 (90, 120, 0);
					//tutorialText.fontSize = 11;
					if (skipTrashingTutorialStep) {
						tutorialText.text = "After you place 5 tags, the image will be replaced.\n" +
							"(Press any button to continue)";
					} else {
						tutorialText.text = "After your peer places 5 tags, the image will be\nreplaced. " +
							"(Press any button to continue)";
					}
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (310, 40f);
					tutorialText.transform.localPosition 
					= new Vector3 (40, -15, 0);
					if (skipTaggingTutorialStep) {
						tutorialText.transform.localPosition 
						= new Vector3 (95, -15, 0);
					}
					stepOfTutorial++;
					if (skipTrashingTutorialStep) {
						stepOfTutorial += 3;
					}
				}
			} else if (stepOfTutorial == 6) { //Showing tags left label:
				if (buttons > 0 && buttonsPrev == 0) { //Change this for the falcon
					//Go to the next step:
					tutorialText.fontSize = 11;
					helpTextContainer.transform.localPosition = new Vector3 (-150, 56, 0);
					tutorialText.text = "If none of the tags appear in the image, you can trash a tag " +
					"and it will be replaced with a new one.\n(Press any button to continue)";
					tutorialArrow.transform.localEulerAngles = new Vector3 (0f, 0f, -45f);
					tutorialArrow.transform.localPosition = new Vector3 (240, 16, 0);
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (550, 40f);
					tutorialText.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (550, 65f);
					tutorialText.transform.localPosition 
					= new Vector3 (7, -15, 0);

					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 7) {
				if (buttons > 0 && buttonsPrev == 0) { //Changes to step where they pick up a tag from the wordbank to be put in the trash
					focusor.SetActive (true);
					focusor.transform.localPosition = new Vector3 (208.12f, -113f, -271.39f);
					focusor.transform.localScale = new Vector3 (10.8f, 3, 3);
					tutorialArrow.transform.localPosition = new Vector3 (160, 16, 0);
					helpTextContainer.transform.localPosition = new Vector3 (-150, 97, 0);
					tutorialText.text = "Select a useless tag from the wordbank";
					tutorialText.GetComponent<RectTransform> ().sizeDelta 
					= new Vector2 (330, 65);
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (250, 20f);
					tutorialText.transform.localPosition 
					= new Vector3 (50, -25f, 0);
					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 8) {
				timeSpentOnStep8 += Time.deltaTime; //To prevent this step from instantly being gone over (this var is being checked in ClickAction.cs)
				if (state.getSelected () != null) { //User's holding a tag
					focusor.transform.localPosition = new Vector3 (347.42f, -111.9f, -271.39f);
					focusor.transform.localScale = new Vector3 (6.1f, 2.22f, 3f);
					tutorialArrow.SetActive (false);
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (330, 40f);
					tutorialText.transform.localPosition 
					= new Vector3 (7, -15f, 0);
					tutorialText.text = "Place it in the bin, and a new word will appear in the wordbank";
					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 9) {
				if (!skipTrashingTutorialStep && state.getSelected() == null || skipTrashingTutorialStep && (buttons > 0 && buttonsPrev == 0)) { //user dropped tag into bin
					tutorialText.fontSize = 12;
					tutorialText.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (400, 40f);
					helpTextPanel.GetComponent<RectTransform> ().sizeDelta
					= new Vector2 (370, 40f);
					tutorialText.text
					= "If your peer has a tag selected, it will highlight red and you\nwill not be able to take it." +
					"(Press any button to continue)";
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (tags[i].text.name.Equals("Tag2"))
                        {
                            tags[i].text.color = Color.red;
                        }
                    }
					focusor.SetActive (false);
					tutorialArrow.SetActive (true);
					tutorialArrow.transform.localEulerAngles = new Vector3 (0f, 0f, -60f);
					helpTextContainer.transform.localPosition = new Vector3 (-235.3f, 147.7f, 0);
					tutorialArrow.transform.localPosition = new Vector3 (60, 100, 0);
					tutorialText.transform.localPosition 
					= new Vector3 (21, -4, 0);
					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 10)
            {
                if (buttons > 0 && buttonsPrev == 0)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (tags[i].text.name.Equals("Tag2"))
                        {
                            tags[i].text.color = Color.black;
                        }
                    }
                    tutorialText.fontSize = 12;
                    tutorialText.GetComponent<RectTransform>().sizeDelta
                    = new Vector2(400, 40f);
                    helpTextPanel.GetComponent<RectTransform>().sizeDelta
                    = new Vector2(370, 40f);
                    tutorialText.text
                    = "You can quit any time you want by pressing the Quit button.\n" +
                    "(Press any button to continue)";
                    focusor.SetActive(false);
                    tutorialArrow.SetActive(true);
                    tutorialArrow.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
                    helpTextContainer.transform.localPosition = new Vector3(-29, -20, 0);
                    tutorialArrow.transform.localPosition = new Vector3(128, -110, 0);
                    tutorialText.transform.localPosition
                    = new Vector3(21, -4, 0);
                    stepOfTutorial++;
                }
            }
            else if (stepOfTutorial == 11) { //Last element of tutorial, reshowing welcome screen basically
				if (buttons > 0 && buttonsPrev == 0) {
					tutorialArrow.SetActive(false);
					helpTextContainer.SetActive (false);
					welcomeScreen.SetActive (true);
					if (skipTaggingTutorialStep) { //If you're the trasher:
						welcomeText.text = "Now let's do a practice level.\n" +
							"You will discard five tags that you can't find in the image.\n" +
							"(Press any button to begin the practice level)";
					} else if (skipTrashingTutorialStep) { //you're the tagger:
						welcomeText.text = "Now let's do a practice level.\n" +
						"You will place five tags onto the image.\n" +
						"(Press any button to begin the practice level)";
					} else { //Single player:
						welcomeText.text = "Now let's do a practice level.\n" +
						"It will be just like a real level but data will not be collected.\n(Press any button to begin the practice level)";
					}
					stepOfTutorial++;
				}
			} else if (stepOfTutorial == 12) {
				if (buttons > 0 && buttonsPrev == 0) { //Change for falcon
					//END OF TUTORIAL:
					inTutorial = false;
					//dataCollector.SetActive (true); Don't collect data for practice level
					welcomeScreen.SetActive (false);

					sequenceIndex = 0; //Reset tags
					for (int i = 0; i < tags.Length; i++) {
						tags[i].setText(wordBank [ SEQUENCE[imageIndex, sequenceIndex] ]);
						sequenceIndex++;
					}
					tagSphere.GetComponent<Renderer> ().material = imageMaterials [imageIndex]; //Start first image
					tagsRemainingText.text = "5 Tags Left"; //Factory Tag prefab from tutorial needs to be deleted:
					foreach(Transform t in ClickAction.sphere.transform)
					{
						Destroy(t.gameObject);
					}
					numTagsRemaining = 5;
					stepOfTutorial++; //End
					inPracticeLevel = true;
					practiceLevelText.SetActive (true);
				}
			}
            buttonsPrev = buttons;
		}
		if (inScreenBeforeExperiment) {
			buttons = state.getButtons ();
			if (buttons > 0 && buttonsPrev == 0) {
				//dataCollector.SetActive (true);
				//welcomeScreen.SetActive (false);
				welcomeText.text = "We are now waiting for the other player to finish the tutorial";
				LoadingIconScript.active = true;
				welcomeText.transform.localPosition = new Vector3 (31.2f, -0.8f, 0f);
				inScreenBeforeExperiment = false;
				waitingForOtherPlayer = true;
			}
			buttonsPrev = buttons;
		}
		if (waitingForOtherPlayer) {
			if (otherPlayerHasFinished) {
				dataCollector.SetActive (true);
				welcomeScreen.SetActive (false);
				waitingForOtherPlayer = false;
				LoadingIconScript.active = false;
			}
		}
		for (int i = 0; i < tags.Length; i++) {
			if (tags [i].isChangingColor) {
				tags [i].text.color = Color.Lerp (tags [i].text.color, Color.black, 0.015f);
			}
			if (tags[i].isChangingColor && tags [i].text.color == Color.black) {
				tags [i].isChangingColor = false;
			}
		}
	}
	//This method can be called from the EventListener script using the GameObject that was clicked on as input:
	public static void replaceTag(GameObject obj, bool clickedImage) {
		//Find tag with this object:
		for (int i = 0; i < tags.Length; i++) {
			if(obj.name == tags[i].text.name) {
				if (inTutorial) {
					if (tutorialWordsIndex < tutorialWords.Length) {
						tags [i].isChangingColor = true;

						tags [i].setText (tutorialWords [tutorialWordsIndex]);
					} else {
						tags [i].setText ("");
					}
				} else {
					if (sequenceIndex < wordBank.Count) {
						tags [i].isChangingColor = true;
						tags [i].text.color = Color.clear;

						tags [i].setText (wordBank [SEQUENCE [imageIndex, sequenceIndex]]);
						sequenceIndex++;
					} else {
						if (clickedImage) {
							tags [i].isChangingColor = true;
							tags [i].text.color = Color.clear;

							tags [i].setText ("");
						} else {
							//Do nothing so you can't throw away tags when there's nothing to replace them
						}
					}
				}
			}
		}
		if (clickedImage || (skipTaggingTutorialStep && inPracticeLevel) || (skipTaggingTutorialStep && continueAfterOtherQuit)) { //If they're the trasher, the practice level should turn over tags left.
			//Handle tags remaining label, image turnover, here:
			if (numTagsRemaining > 2) { //Plural "tags remaining" vs singular "tag remaining" (minor detail):
				numTagsRemaining--;

				if (skipTaggingTutorialStep && inPracticeLevel) {
				} else {

					tagsRemainingText.text = numTagsRemaining + " Tags Left";
				}
			} else {
				numTagsRemaining--;
				if (numTagsRemaining == 1) {
					if (!(skipTaggingTutorialStep && inPracticeLevel)) {
						tagsRemainingText.text = numTagsRemaining + " Tag Left";
					}
				} else {
					if (imageIndex == imageMaterials.Length - 1) { //On last image, then quit:
						QuitGameScript.quitGame();
					}
					if (!inTutorial) {
						if (!inPracticeLevel) { //Are we not in the practice level:
							DataCollector.Flush ();
						}
						if (!skipTaggingTutorialStep) { //Only set these if you're the tagger:
							Transform lastTag = tagSphere.transform.GetChild (tagSphere.transform.childCount - 1);

							positionLastTag = lastTag.localPosition;
							rotationLastTag = lastTag.localRotation.eulerAngles;
							scaleLastTag = lastTag.localScale;
						}

						if (ClickAction.cursorTag != null) {
							Destroy (ClickAction.cursorTag);
							ClickAction.cursorTag = null;
						}

						foreach (Transform t in tagSphere.transform) {
							Destroy (t.gameObject, 0.08f);
						}

						for (int i = 0; i < ClickAction.trashedTags.Count; i++) {
							Destroy (ClickAction.trashedTags [i]);
						}
						ClickAction.trashedTags.Clear();

						numTagsRemaining = 5;
						tagsRemainingText.text = numTagsRemaining + " Tags Left";
						imageIndex++;
						tagSphere.GetComponent<Renderer> ().material = imageMaterials [imageIndex];
						sequenceIndex = 0;
						for (int tagsIndex = 0; tagsIndex < tags.Length; tagsIndex++) {
							tags [tagsIndex].setText (wordBank [SEQUENCE [imageIndex, sequenceIndex]]);
							tags [tagsIndex].text.color = Color.black;
							sequenceIndex++;
						}
						if (inPracticeLevel) {
							practiceLevelText.SetActive (false);
							welcomeText.text = "You have completed the practice level.\nPress any button to " +
								"begin data collection."; 
							welcomeScreen.SetActive (true);
							inScreenBeforeExperiment = true;
							inPracticeLevel = false;
						}
					}
				}
			}
		}
	}
}
/*
 * Container class for each tag which contains the 
 * Tag GameObject and the Text object: 
 */
public class Tag {
	public GameObject tag;
	public Text text;
	public bool isChangingColor = false; //This will allow a tag coming in from the word bank to gradually fade to black text from clear
	public Tag(GameObject tag, int index) {
		this.tag = tag;
		this.text = tag.GetComponentInChildren<Text>();
        this.text.name = "Tag" + index; // Give the Text object an identifier
	}
	public string getText() {
		return text.text;
	}
	public void setText(string next_text) {
		tag.name = next_text;
		//text.name = next_text; //The Text Object name acts as the identifier when you click on it and should be unique
		text.text = next_text;
	}
}