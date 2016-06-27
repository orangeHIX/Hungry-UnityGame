using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public float panelWidth;
    public float panelHeight;

    Vector3 newPosition;

    IEnumerator move()
    {
        while (!transform.localPosition.Equals(newPosition))
        {

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, 2000 * Time.deltaTime);
            //Debug.Log("Camera Position: " + transform.localPosition.ToString());
            yield return null;
        }
    }

    public void moveTo(Vector3 newPosition)
    {
        this.newPosition = newPosition;
        StartCoroutine("move");
    }

    public void moveRight()
    {
        moveTo(transform.localPosition + new Vector3(panelWidth, 0, 0));
    }

    public void moveLeft()
    {
        moveTo(transform.localPosition + new Vector3(-panelWidth, 0, 0));
    }

    public void moveBottom()
    {
        moveTo(transform.localPosition + new Vector3(0, -panelHeight, 0));
    }

    public void moveTop()
    {
        moveTo(transform.localPosition + new Vector3(0, panelHeight, 0));
    }

}
