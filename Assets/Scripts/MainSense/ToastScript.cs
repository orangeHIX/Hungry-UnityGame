using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Toast
{
    static ToastScript script;
    public static void Pop(string info) {
        if (script == null)
        {
            GameObject obj = GameObject.Find("ToastSystem");
            if (obj != null)
            {
                script = obj.GetComponent<ToastScript>();
                if (script == null) {
                    Debug.LogError("can not find ToastSystem with ToastScript");
                }
            }
        }
        if(script != null) {
            script.Toast(info);
        }
    }
}

public class ToastScript : MonoBehaviour
{
    public GameObject toastCanvas;
    public GameObject toastTextPrefab;

    const int MaxPoolSize = 5;
    List<ToastTextScript> toastTextPool;

    //private Vector2 rectTransformSizeMemo;
    //private RectTransform rectTransform;

    void Awake() {
        toastTextPool = new List<ToastTextScript>();
        ToastTextScript[] scripts = toastCanvas.transform.GetComponentsInChildren<ToastTextScript>();
        for (int i = 0; i < MaxPoolSize && i < scripts.Length; i++) {
            toastTextPool.Add(scripts[i]);
        }
        for (int i = toastCanvas.transform.childCount; i < MaxPoolSize; i++)
        {
            GameObject item = Instantiate(toastTextPrefab);
            item.transform.SetParent(toastCanvas.transform);
            toastTextPool.Add(item.GetComponent<ToastTextScript>());
        }
    }

    public void Toast(string str)
    {
        ToastTextScript script = toastTextPool.Find(x => x.isIdle());
        if (script != null)
        {
            script.PlayAnimation(str);
        }
    }

}
