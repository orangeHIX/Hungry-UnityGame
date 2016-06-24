using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Battle
{
    public class FloatText : MonoBehaviour
    {
        private float time = 0;
        void Start()
        {

        }

        public bool IsUp;

        void Update()
        {
            if (IsUp)
            {
                this.transform.localPosition += Vector3.down * Time.deltaTime * 20;
            }
            else
            {
                this.transform.localPosition -= Vector3.down * Time.deltaTime * 20;
            }
            time += Time.deltaTime;
            if (time > 2)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        public void SetText(string _str)
        {
            this.GetComponent<Text>().text = _str;
        }
    }

}