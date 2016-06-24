using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextHarvestTimeScript : MonoBehaviour{


    public Text nextHarvestText;
    public const float HarvestTimeInSec = 11;
    public float nextHarvestTime
    {
        get
        {
            return HarvestTimeInSec - (Time.time - startTime);
        }
    }

    HRManagerScript HRManagerScript;

    float startTime;
    bool isFirstUpdate;


    // Use this for initialization
    void Start()
    {
        HRManagerScript = transform.GetComponent<HRManagerScript>();
        isFirstUpdate = true;
    }

    // Update is called once per frame
    void Update () {
        if (isFirstUpdate) {
            startTime = Time.time;
            ChangeNextHarvestText();
            isFirstUpdate = false;
            StartCoroutine("Fade");
        }
        if (nextHarvestTime <= 0) {
            if ( HRManagerScript != null)
            {
                HRManagerScript.Harvest();
            }
            startTime = Time.time + nextHarvestTime;
        }
	}

    IEnumerator Fade()
    {
        while(true)
        {
            ChangeNextHarvestText();
            yield return new WaitForSeconds(.1f);
        }
    }

    void ChangeNextHarvestText()
    {
        nextHarvestText.text = string.Format(
            "下一次收入 {0:00}:{1:00}",
            nextHarvestTime / 60,
            nextHarvestTime % 60);
    }
}
