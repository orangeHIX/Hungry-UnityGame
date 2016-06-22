using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBarButton : MonoBehaviour {

    private Vector3 localPositionMemo;

    RectTransform rectTransform;
    Transform barImage;

	// Use this for initialization
	void Start () {
        barImage = transform.FindChild("MaskImage").FindChild("BarImage");
        localPositionMemo = barImage.localPosition;
        rectTransform = transform.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator progress()
    {
        Button b = transform.GetComponent<Button>();
        b.enabled = false;
        float width = rectTransform.rect.width;
        Vector3 newPosition = barImage.localPosition + new Vector3(-width, 0, 0);
        while (barImage.localPosition.x != newPosition.x)
        {
            barImage.localPosition = Vector3.MoveTowards(barImage.localPosition, newPosition, width * Time.deltaTime);
            yield return null;
        }
        barImage.localPosition = localPositionMemo;
        b.enabled = true;
        yield return null;
    }

    public void ClickButton() {
        StartCoroutine("progress");
    }
}
