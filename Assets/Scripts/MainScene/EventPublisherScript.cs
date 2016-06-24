using UnityEngine;
using System.Collections;

public class GameEventPublisher
{
    static EventPublisherScript script;
    public static void Publish(string info)
    {
        if (script == null)
        {
            GameObject obj = GameObject.Find("EventPublisher");
            if (obj != null)
            {
                script = obj.GetComponent<EventPublisherScript>();
                if (script == null)
                {
                    Debug.LogError("can not find EventPublisher with EventPublisherScript");
                }
            }
        }
        if (script != null)
        {
            script.AddEvent(info);
        }
    }
}
public class EventPublisherScript : MonoBehaviour {

    public delegate void GameInfoEventHandler(string info);

    public event GameInfoEventHandler GameInfo;

    public void AddEvent(string eventStr) {
        if (GameInfo != null) {
            GameInfo(eventStr);
        }
    }

}
