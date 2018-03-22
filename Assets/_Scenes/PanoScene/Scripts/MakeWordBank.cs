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

	//Numbers 0-47 (elements of .csv file) in a random order, different predetermined random indexes for all 16 images:
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
			4, 10, 33, 16, 18, 29, 13, 34, 14, 41, 15, 24, 43, 20, 19, 7, 27, 6, 8, 37, 1, 11, 36, 45, 30, 31, 26, 3, 9, 47, 21, 32, 28, 38, 25, 2, 17, 46, 40, 23, 42, 35, 44, 39, 12, 5, 22, 0
		}, {
			5, 25, 42, 20, 29, 22, 27, 21, 41, 38, 33, 47, 16, 11, 7, 30, 10, 39, 46, 37, 31, 9, 2, 3, 34, 36, 45, 4, 14, 12, 35, 26, 44, 28, 19, 0, 24, 8, 17, 15, 23, 32, 13, 1, 18, 40, 6, 43
		}, {
			45, 47, 14, 40, 16, 33, 42, 4, 6, 2, 36, 24, 31, 37, 23, 5, 18, 39, 13, 3, 15, 29, 0, 9, 28, 7, 46, 22, 35, 44, 41, 1, 30, 34, 25, 11, 20, 19, 8, 21, 17, 26, 10, 38, 43, 32, 12, 27
		}, {
			4, 33, 15, 25, 5, 16, 29, 9, 32, 3, 24, 45, 30, 18, 42, 46, 28, 34, 10, 7, 20, 19, 31, 0, 37, 36, 12, 27, 40, 44, 26, 38, 41, 13, 22, 23, 35, 2, 39, 17, 6, 14, 47, 21, 8, 1, 11, 43
		}, {
			5, 10, 43, 18, 44, 15, 36, 6, 40, 0, 23, 47, 32, 35, 30, 11, 4, 14, 12, 34, 20, 21, 28, 2, 33, 1, 27, 7, 42, 19, 13, 37, 29, 22, 26, 38, 24, 17, 16, 41, 25, 3, 46, 31, 9, 8, 39, 45
		}, {
			14, 39, 7, 26, 41, 27, 36, 33, 11, 12, 18, 35, 25, 45, 29, 15, 40, 19, 21, 16, 5, 47, 44, 0, 23, 43, 4, 13, 10, 37, 42, 24, 28, 30, 6, 20, 38, 9, 17, 31, 32, 2, 34, 22, 1, 46, 8, 3
		} 
	};

	static int sequenceIndex = 0; //Indicates which element of predetermined sequence we're on, should reset to 0 every turnover
	static int imageIndex = 0; //Index for which 360 Material to use as well as which predetermined random int sequence to use

	static int numTagsRemaining = 5;


	public static GameObject tagSphere;
	public Material[] imageMaterialsToDragIn; 
	public static Material[] imageMaterials;

	public static Text tagsRemainingText;

	public static List<string> wordBank = new List<string>();

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

		// **************** IMAGE 1: *******************************
		for (int i = 0; i < tags.Length; i++) {
			tags[i].setText(wordBank [ SEQUENCE[imageIndex, sequenceIndex] ]);
			sequenceIndex++;
		}
		tagSphere.GetComponent<Renderer> ().material = imageMaterials [imageIndex];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//This method can be called from the EventListener script using the GameObject that was clicked on as input:
	public static void replaceTag(GameObject obj, bool clickedImage) {
		//Find tag with this object:
		for (int i = 0; i < tags.Length; i++) {
			if(obj.name == tags[i].text.name) {
				if (sequenceIndex < wordBank.Count) {
					tags[i].setText(wordBank [ SEQUENCE[imageIndex, sequenceIndex] ]);
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
		if (clickedImage) {
			//Handle tags remaining label, image turnover, here:
			if (numTagsRemaining > 2) { //Plural "tags remaining" vs singular "tag remaining" (minor detail):
				numTagsRemaining--;
				tagsRemainingText.text = numTagsRemaining + " Tags Left";
			} else {
				numTagsRemaining--;
				if (numTagsRemaining == 1) {
					tagsRemainingText.text = numTagsRemaining + " Tag Left";
				} else {

                    DataCollector.Flush();
					if (imageIndex == imageMaterials.Length - 1) { //On last image, then quit:
						Application.Quit ();
					}
                    //Turnover image, delete tags left out on image:
                    /*for (int prefabIndex = 0; prefabIndex < 5; prefabIndex++) {
						Destroy (GameObject.Find ("TagPrefab").GetComponent<Renderer>());
					}*/
                    foreach(Transform t in GameObject.Find("TagSphere").transform)
                    {
                        Destroy(t.gameObject);
                    }
					numTagsRemaining = 5;
					tagsRemainingText.text = numTagsRemaining + " Tags Left";
					imageIndex++;
					tagSphere.GetComponent<Renderer> ().material = imageMaterials [imageIndex];
					sequenceIndex = 0;
					for (int tagsIndex = 0; tagsIndex < tags.Length; tagsIndex++) {
						tags[tagsIndex].setText(wordBank [ SEQUENCE[imageIndex, sequenceIndex] ]);
						sequenceIndex++;
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