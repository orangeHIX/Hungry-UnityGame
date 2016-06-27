using UnityEngine;
using System.Collections;

public class NavigationScript : MonoBehaviour {

    public GameObject mainCamera;
    CameraScript cameraScript;


    public GameObject currVisibleCanvas;
    public GameObject mainPanelCanvas;
    public GameObject collectPanelCanvas;
    public GameObject adventruePanelCanvas;
    public GameObject storePanelCanvas;

    public RectTransform mainPanelRectTran;
    public RectTransform collectPanelRectTran;
    public RectTransform adventruePanelRectTran;
    public RectTransform storePanelRectTran;
    // Use this for initialization
    void Start() {

        Vector3 mainPos = Vector3.zero;
        float mainRectWidth = 0;
        float mainRectHeight = 0;
        if (mainPanelCanvas != null) {
            mainPanelRectTran = mainPanelCanvas.GetComponent<RectTransform>();
            mainPos = mainPanelRectTran.position;
            mainRectWidth =mainPanelRectTran.rect.width;
            mainRectHeight = mainPanelRectTran.rect.height;
        }
        if (collectPanelCanvas != null) {
            collectPanelRectTran = collectPanelCanvas.GetComponent<RectTransform>();
            collectPanelRectTran.position = mainPos + new Vector3(mainRectWidth,0,0);
            //collectPanelCanvas.SetActive(false);
        }
        if (adventruePanelCanvas != null) {
            adventruePanelRectTran = adventruePanelCanvas.GetComponent<RectTransform>();
            adventruePanelRectTran.position = mainPos + new Vector3( -mainRectWidth, 0, 0);
            //adventruePanelCanvas.SetActive(false);
        }
        if (storePanelCanvas != null) {
            storePanelRectTran = storePanelCanvas.GetComponent<RectTransform>();
            storePanelRectTran.position = mainPos + new Vector3(0, -mainRectHeight, 0);
            //storePanelCanvas.SetActive(false);
        }

        if (mainCamera != null)
        {
            cameraScript = mainCamera.GetComponent<CameraScript>();
            cameraScript.panelWidth = mainRectWidth;
            cameraScript.panelHeight = mainRectHeight;
        }

        currVisibleCanvas = mainPanelCanvas;
    }

    public void GotoMainPanel()
    {
        MoveCameratoPanel(mainPanelRectTran);
        currVisibleCanvas = mainPanelCanvas;
    }

    public void GotoCollectPanel()
    {
        //collectPanelCanvas.SetActive(true);
        MoveCameratoPanel(collectPanelRectTran);
        currVisibleCanvas = collectPanelCanvas;
    }


    public void GotoAdventruePanel() {
        MoveCameratoPanel(adventruePanelRectTran);
        currVisibleCanvas = adventruePanelCanvas;
    }

    public void GotoStorePanel() {
        MoveCameratoPanel(storePanelRectTran);
        currVisibleCanvas = storePanelCanvas;
    }

    private void MoveCameratoPanel(RectTransform rectTran) {

        Vector3 v = new Vector3(
            rectTran.position.x,
            rectTran.position.y,
            cameraScript.transform.position.z);

        cameraScript.moveTo(v);
    }
}
