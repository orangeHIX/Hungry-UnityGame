using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanel {
    static InfoPanelScript script;

    public static void Pop(string info)
    {
        if (script == null)
        {
            GameObject obj = GameObject.Find("InfoPanel");
            if (obj != null)
            {
                script = obj.GetComponent<InfoPanelScript>();
                if (script == null)
                {
                    Debug.LogError("can not find InfoPanel with InfoPanelScript");
                }
            }
        }
        if (script != null)
        {
            script.SetText(info);
        }
    }
}


public class InfoPanelScript : MonoBehaviour
{

    public Text text;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update() {

        //click anywhere to hide InfoPanel
        if (Input.GetMouseButtonDown(0)) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pop") ) {
                anim.SetTrigger("hide");
            }
        }
    }

    public void SetText(string s)
    {
        if (text != null)
        {
            text.text = s;
            anim.SetTrigger("show");
        }
    }
}
