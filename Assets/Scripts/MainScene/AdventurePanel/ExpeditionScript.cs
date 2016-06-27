using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExpeditionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Go() {
        SceneManager.LoadScene("battle");
    }
}
