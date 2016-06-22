using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogScript : MonoBehaviour {

    public GameObject eventPublisher;
    public Text text;

	// Use this for initialization
	void Start () {
        if (eventPublisher != null) {
            EventPublisherScript script = eventPublisher.GetComponent<EventPublisherScript>();
            if (script != null) {
                script.GameInfo += AddText;
            }
        }
        if (text != null) {
            text.text = "执行官，欢迎您的到来。";
        }
	}

    //// Update is called once per frame
    //void Update () {

    //}

    public void AddText(string s) {
        if (text != null)
        {
            text.text = s + "\n" + text.text;
        }
    }
            

}
