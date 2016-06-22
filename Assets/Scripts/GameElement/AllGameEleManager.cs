using UnityEngine;
using System.Collections.Generic;


public class AllGameEleManager : MonoBehaviour {

    public static string LoadResourceTextfile(string path)
    {
        string filePath = "SetupData/" + path.Replace(".json", "");

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }

    public static void saveJSON(string path, System.Object o)
    {
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
