using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public const int CameraWidth = 500;
    public const int CameraHeight = 800;
    Vector3 newPosition;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator move()
    {
        while (!transform.localPosition.Equals(newPosition))
        {

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, 2000 * Time.deltaTime);
            //Debug.Log("Camera Position: " + transform.localPosition.ToString());
            yield return null;
        }
    }

    void moveTo(Vector3 newPosition)
    {
        this.newPosition = newPosition;
        StartCoroutine("move");
    }

    public void moveRight()
    {
        moveTo(transform.localPosition + new Vector3(CameraWidth, 0, 0));
    }

    public void moveLeft()
    {
        moveTo(transform.localPosition + new Vector3(-CameraWidth, 0, 0));
    }

    public void moveBottom()
    {
        moveTo(transform.localPosition + new Vector3(0, -CameraHeight, 0));
    }

    public void moveTop()
    {
        moveTo(transform.localPosition + new Vector3(0, CameraHeight, 0));
    }

}
