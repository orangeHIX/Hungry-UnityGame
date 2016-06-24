using UnityEngine;
using System.Collections;
namespace Battle
{
    public class Effect : MonoBehaviour
    {
        public float EndTime;
        private float time;
        public bool IsStart;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (IsStart)
            {
                time += Time.deltaTime;
                if (time >= EndTime)
                {
                    IsStart = false;
                    this.gameObject.SetActive(false);
                    time = 0;
                }
            }
        }
    }

}