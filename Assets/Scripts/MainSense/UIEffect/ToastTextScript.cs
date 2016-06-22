using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToastTextScript : MonoBehaviour {

    Animator anim;
//    Animation anim;
    Text text;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
//        anim = GetComponent<Animation>();
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //anim.CrossFade("TextPop");
        //Debug.Log(transform.position);
        //anim.SetBool("isShow", true);
    }

    public bool isIdle() {
        if (anim != null)
        {
            return anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        }
        return false;
    }

    public void PlayAnimation(string str) {
        text.text = str;
        if (anim != null && text != null)
        {
            anim.SetTrigger("Pop");
        }
    }

}
