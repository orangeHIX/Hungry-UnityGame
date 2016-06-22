using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {


    public float health;
    public float experience;
	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
