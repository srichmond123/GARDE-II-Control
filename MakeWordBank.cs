using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MakeWordBank : MonoBehaviour {

	//I'm putting the CSV files in the directory Assets/Tags, 
	//I'm only using image1.csv for now..

	static string image1Path = "Assets/Tags/image1.csv";

	//For the next two arrays, I drag and dropped the objects from the Editor into the script window in the inspector - 
	//This script takes that data and automatically makes however many "Tag" objects (class below) are needed:
	public GameObject[] tagGameObjects;
	public Text[] textObjects;


	public static List<string> wordBank = new List<string>();

	//Array of the container class I made below for a "Tag" object - since it's static, 
	//you can have an eventlistener on another class and call methods like MakeWordBank.tags[i].setText,
	//which will change the name of the Tag GameObject and the text of the Text object for that tag..
	//Also, the List<string> object above is static and can be called in the same way.. To get
	//new names you should pick a random index from the List wordBank then call the RemoveAt function
	//for that index
	public static Tag[] tags;

	void Start () {
		tags = new Tag[tagGameObjects.Length];
		for (int i = 0; i < tags.Length; i++) {
			tags [i] = new Tag (tagGameObjects [i], textObjects [i]);
		}
		//Read CSV File:
		using (StreamReader sr = new StreamReader(image1Path))
		{
			string line;

			while ((line = sr.ReadLine()) != null)
			{
				string[] parts = line.Split(',');

				string elem = parts[parts.Length - 1]; //Last column of .csv must be the tag names
				if (!string.Equals(elem, "")) {
					wordBank.Add(elem);
				}
			}
		}
		wordBank.RemoveAt (0); //<-- Column name

		for (int i = 0; i < tags.Length; i++) {
			//I cleaned up the CSV file for image1 (removing duplicates, underscores, etc)
			int index = (int) (Random.value * wordBank.Count);
			tags[i].setText(wordBank [index]);
			wordBank.RemoveAt (index); //Pops word from wordbank so it can't be used again
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
/*
 * Container class for each tag which contains the 
 * Tag GameObject and the Text object:
 */
public class Tag {
	public GameObject tag;
	public Text text;
	public Tag(GameObject tag, Text text) {
		this.tag = tag;
		this.text = text;
	}
	public string getText() {
		return text.text;
	}
	public void setText(string next_text) {
		tag.name = next_text;
		text.name = next_text;
		text.text = next_text;
	}
}