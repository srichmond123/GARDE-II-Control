using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MakeWordBank : MonoBehaviour {

	//I'm putting the CSV files in the directory Assets/Tags, 
	//Using the final wordbank Roni gave me:

	static string image1Path = "Assets/Tags/wordbank.csv";
		private List<GameObject> tagGameObjects;

    // The Text object will be a child of the panel representing that tag, so it is fine to have one array representing the GameObject and the Tag for each tag
	//public Text[] textObjects;

	//Numbers 0-47 (elements of .csv file) in a random order:
	public static int[] SEQUENCE_1 = { 44, 43, 2, 23, 30, 46, 3, 27, 45, 17, 41, 1, 42, 
		29, 12, 34, 16, 6, 13, 37, 47, 25, 21, 22, 19, 33, 32, 11, 26, 7, 24, 0, 10, 
		39, 8, 36, 20, 4, 31, 14, 40, 35, 18, 38, 15, 28, 9, 5
	};
	static int sequenceIndex = 0;

	public static List<string> wordBank = new List<string>();

	//Array of the container class I made below for a "Tag" object - since it's static, 
	//you can have an eventlistener on another class and call methods like MakeWordBank.replaceTag(GameObject obj)
	//which replaces the Tag with the next Tag name in line, uploaded from the .csv file.
	//This script should work fine, the important thing is that the Text objects whose parents are the
	//tag GameObjects should have unique names (doesn't matter what the names are), the parent
	//GameObjects' names can be changed though with no problem
	public static Tag[] tags;

	void Start () {
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
		using (StreamReader sr = new StreamReader(image1Path))
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

		for (int i = 0; i < tags.Length; i++) {
			tags[i].setText(wordBank [ SEQUENCE_1[sequenceIndex] ]);
			sequenceIndex++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//This method can be called from the EventListener script using the GameObject that was clicked on as input:
	public static void replaceTag(GameObject obj, bool clickedImage) {
		//Find tag with this object:
		for (int i = 0; i < tags.Length; i++) {
			if(obj.name == tags[i].text.name) {
				if (sequenceIndex < SEQUENCE_1.Length) {
					tags[i].setText(wordBank [ SEQUENCE_1[sequenceIndex] ]);
					sequenceIndex++;
				} else {
					if (clickedImage) {
						tags [i].setText ("");
					} else {
						//Do nothing so you can't throw away tags when there's nothing to replace them
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